#!/bin/bash

#Build all the .NET Docker Images

#Run the docker compose and specify a project name to keep everything organized
docker-compose -p MTDemo -f docker-compose.yml up --build
