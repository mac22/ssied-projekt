require("colors");

var fs = require("fs"),
    glob = require("glob"),
    argv = require("optimist")
        .usage("Usage: $0 --src source_directory --dest destinsation_directory --split split_pattern")
        .demand("dest")
        .default("src", "../datasets/webkb")
        .default("split", "test.2:training.8")
        .describe("src", "Datasets directory")
        .describe("dest", "Where to put results")
        .describe("split", "How to split data, eg. test.2:training.8 (test data 20%, training data 80%)")
        .argv,

  util = require("util"),

  log = function(value) {
    if (!argv.q) {
    console.log(value);
    }
  },

  dieIfError = function(err) {
    if (err) {
      if (!argv.q) {
        console.error(err.red);
      }

      process.exit(-1);
    }
  };

fs.copy = function (src, dst, cb) {
  function copy(err) {
    var is, os;

    fs.stat(src,  function (err) {
                    if (err) {
                      return cb(err);
                    }

                    is = fs.createReadStream(src);
                    os = fs.createWriteStream(dst);

                    util.pump(is, os, cb);
                  });
  }

  fs.stat(dst, copy);
};

fs.mkdir(argv.dest);

var sets = argv.split.split(":");

sets = sets.map(function(set) {
                  var _set = set.split(".");

                  _set[1] = (_set[1]/10.0)
                  fs.mkdir(argv.dest+"/"+_set[0]);

                  return _set;
                });

console.log(sets);

glob(argv.src + "/*", argv.D, function (err, files) {
  dieIfError(err);

  files.forEach(function(file) {
                  console.log(util.format("\t@@@ %s", file));

                  glob(file+"/*/*", argv.D, function (err, files) {
                                              dieIfError(err);

                                              files.forEach(function(file) {
                                                              var shoot = Math.random(),
                                                                  prev_treshold = 0.0;

                                                              sets.every(function(set) {
                                                                if(prev_treshold < shoot && shoot < (set[1]+prev_treshold)) {
                                                                  var path = file.split("/"),
                                                                      dir = argv.dest+"/" + set[0] + "/" + path[path.length - 3];

                                                                  fs.mkdir(dir);
                                                                  fs.copy(file, dir+"/"+path[path.length - 1]);

                                                                  return false;
                                                                }

                                                                prev_treshold += set[1];
                                                                return true;
                                                              });
                                                            });
                                            });
                });
});