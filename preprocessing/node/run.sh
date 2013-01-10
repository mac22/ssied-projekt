#!/bin/sh
N=4
node main.js -D --ommitNumbers -L $N -q              > results/TFIDF-per-doc

node main.js -C course --ommitNumbers -L $N -q       > results/TFIDF-course
node main.js -C department --ommitNumbers -L $N -q   > results/TFIDF-department
node main.js -C faculty --ommitNumbers -L $N -q      > results/TFIDF-faculty
node main.js -C other --ommitNumbers -L $N -q        > results/TFIDF-other
node main.js -C projecti --ommitNumbers -L $N -q     > results/TFIDF-project
node main.js -C staff --ommitNumbers -L $N -q        > results/TFIDF-staff
node main.js -C student --ommitNumbers -L $N -q      > results/TFIDF-student
