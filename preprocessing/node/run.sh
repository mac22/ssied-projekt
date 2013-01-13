#!/bin/sh

# Install node.
if [ -x "node" ]; then
  sudo apt-get install python-software-properties
  sudo add-apt-repository ppa:chris-lea/node.js
  sudo apt-get update
  sudo apt-get install nodejs npm
fi

# Install dependencies.
npm install

# Checking datasets.
if [ ! -d "../datasets/webkb/" ]; then
  echo "MISSING DATASETS IN ../datasets/webkb :("
  exit
fi

# Splitting.
node splitter.js --dest ./splitted_datasets

# Generate dictionaries.
N=4

node main.js -D --omitNumbers -L $N -q              > dictionaries/TFIDF-per-doc

node main.js -C course --omitNumbers -L $N -q       > dictionaries/TFIDF-course
node main.js -C department --omitNumbers -L $N -q   > dictionaries/TFIDF-department
node main.js -C faculty --omitNumbers -L $N -q      > dictionaries/TFIDF-faculty
node main.js -C other --omitNumbers -L $N -q        > dictionaries/TFIDF-other
node main.js -C project --omitNumbers -L $N -q      > dictionaries/TFIDF-project
node main.js -C staff --omitNumbers -L $N -q        > dictionaries/TFIDF-staff
node main.js -C student --omitNumbers -L $N -q      > dictionaries/TFIDF-student

# Converting to CSV.