#!/bin/bash
USER_NAME=arasz
HOST=207.154.213.152 
DATABASE=NaCoDoKina.PROD

set -ev

pg_dump --clean --create --host=$HOST --username=$USER_NAME $DATABASE > backups/$DATABASE.dump