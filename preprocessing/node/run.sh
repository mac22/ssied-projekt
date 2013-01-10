#!/bin/sh
node main.js -D --ommitNumbers -L 4 -q              > results/TFIDF-per-doc

node main.js -C course --ommitNumbers -L 4 -q       > results/TFIDF-course
node main.js -C department --ommitNumbers -L 4 -q   > results/TFIDF-department
node main.js -C faculty --ommitNumbers -L 4 -q      > results/TFIDF-faculty
node main.js -C other --ommitNumbers -L 4 -q        > results/TFIDF-other
node main.js -C projecti --ommitNumbers -L 4 -q     > results/TFIDF-project
node main.js -C staff --ommitNumbers -L 4 -q        > results/TFIDF-staff
node main.js -C student --ommitNumbers -L 4 -q      > results/TFIDF-student
