#!/bin/bash
USER_NAME=postgres
HOST=localhost 
SOURCE_DATABASE=NaCoDoKina.PROD
TARGET_DATABASE=NaCoDoKina.DEV
set -ev

psql --echo-errors --host=$HOST --username=$USER_NAME $TARGET_DATABASE < backups/$SOURCE_DATABASE.dump