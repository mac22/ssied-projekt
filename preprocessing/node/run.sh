#!/bin/sh

# Install node.
node -v >/dev/null 2>&1 || {
  echo >&2 "I require node but it's not installed. Installing...";
  sudo apt-get install python-software-properties
  sudo add-apt-repository ppa:chris-lea/node.js
  sudo apt-get update
  sudo apt-get install nodejs npm
}

# Install dependencies.
if [ ! -e "node_modules/" ]; then
  npm install -q --no-color
fi

# Checking datasets.
if [ ! -d "../datasets/webkb/" ]; then
  echo "MISSING DATASETS IN ../datasets/webkb :("
  exit
fi

# Splitting.
node splitter.js --dest ./splitted_datasets       > splitted_datasets/output

# Generate dictionaries.
N=4

node main.js -D --omitNumbers -L $N -q            > dictionaries/TFIDF-per-doc

node main.js -C course --omitNumbers -L $N -q     > dictionaries/TFIDF-course
node main.js -C department --omitNumbers -L $N -q > dictionaries/TFIDF-department
node main.js -C faculty --omitNumbers -L $N -q    > dictionaries/TFIDF-faculty
node main.js -C other --omitNumbers -L $N -q      > dictionaries/TFIDF-other
node main.js -C project --omitNumbers -L $N -q    > dictionaries/TFIDF-project
node main.js -C staff --omitNumbers -L $N -q      > dictionaries/TFIDF-staff
node main.js -C student --omitNumbers -L $N -q    > dictionaries/TFIDF-student

# Converting to CSV.
rm -f csv/*

mono csv-converter.exe dictionaries/TFIDF-per-doc csv/CSV-per-doc

mono csv-converter.exe dictionaries/TFIDF-course csv/CSV-course
mono csv-converter.exe dictionaries/TFIDF-department csv/CSV-department
mono csv-converter.exe dictionaries/TFIDF-faculty csv/CSV-faculty
mono csv-converter.exe dictionaries/TFIDF-other csv/CSV-other
mono csv-converter.exe dictionaries/TFIDF-project csv/CSV-project
mono csv-converter.exe dictionaries/TFIDF-staff csv/CSV-staff
mono csv-converter.exe dictionaries/TFIDF-student csv/CSV-student