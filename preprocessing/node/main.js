
/*jshint es5: true*/

require("colors");

var fs = require("fs"),
    htmlparser = require("htmlparser"),
    natural = require("natural"),
    glob = require("glob"),
    argv = require("optimist")
            .usage("Usage: $0 [-C className] [--threshold 6.0] [-q] [-D]")
            .boolean([ "q" ])
            .default("C", "course")
            .default("threshold", null)
            .describe("q", "quiet output")
            .describe("D", "calculate TF-IDF per document")
            .describe("threshold", "TF-IDF threshold filtering value")
            .describe("C", "class name")
            .argv,
    util = require("util"),

    TfIdf = natural.TfIdf,
    tfidf = new TfIdf(),

    dieIfError = function(err) {
      if (err) {
        if (!argv.q) {
          console.error(err.red);
        }

        process.exit(-1);
      }
    },

    log = function(value) {
      if (!argv.q) {
        console.log(value);
      }
    },

    EOL = "\n",
    mimeLastPosition = 6,

    handler = new htmlparser.DefaultHandler(dieIfError),
    parser = new htmlparser.Parser(handler),

    isText = function(element) {
      return element.type === "text";
    },

    isValid = function(element) {
      return !!element.raw;
    },

    extractMimeHeaders = function(data) {
      return data.split(EOL).slice(mimeLastPosition).join(EOL);
    },

    extractedText = [],
    extractText = function(array) {
      array.forEach(function(element, index) {
        if (isText(element) && isValid(element)) {
          extractedText.push(element.raw.trim());
        }

        if (element.hasOwnProperty("children")) {
          extractText(element.children);
        }
      });
    },

    calculateAndPrintTFIDF = function() {
      tfidf.addDocument(extractedText.join(" "));
      tfidf.listTerms(0).forEach(function(element) {
        var result = util.format("%s %d", element.term, element.tfidf);

        if (!argv.q) {
          result = result.green;
        }

        if (!!argv.threshold) {
          if (element.tfidf > argv.threshold) {
            console.log(result);
          }
        } else {
          console.log(result);
        }
      });
    };

glob(util.format("../datasets/webkb/%s**/*", argv.D ? "" : util.format("%s/", argv.C)), function (err, files) {
  dieIfError(err);

  files.forEach(function(file) {
    var stats = fs.statSync(file),
        data;

    if (stats.isFile()) {
      log("\tProcessing... ".yellow + file);

      data = fs.readFileSync(file, "UTF-8");

      parser.parseComplete(extractMimeHeaders(data));
      extractText(handler.dom);

      if (argv.D) {
        if (argv.q) {
          console.log(util.format("\t@@@ %s", file));
        }

        calculateAndPrintTFIDF();
        tfidf = new TfIdf();
        extractedText = [];
      }
    }
  });

  if (!argv.D) {
    calculateAndPrintTFIDF();
  }
});

log("Extractor is running...".rainbow);