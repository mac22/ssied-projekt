#!/bin/sh
node main.js -D -q > results/TFIDF-per-doc

node main.js -C course -q > results/TFIDF-course
node main.js -C department -q > results/TFIDF-department
node main.js -C faculty -q > results/TFIDF-faculty
node main.js -C other -q > results/TFIDF-other
node main.js -C project -q > results/TFIDF-project
node main.js -C staff -q > results/TFIDF-staff
node main.js -C student -q > results/TFIDF-student
