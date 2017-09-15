
# WORK IN PROGRESS

# NaCoDoKina.Api
Api for NaCoDoKina project. Contains all bussines logic. In first version created as simple monlitic  n-tier web application.

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![Master Branch Build Status](https://travis-ci.com/Arasz/NaCoDoKina.Api.svg?token=jsB52VbhD4YcywUiDpoV&branch=master)](https://travis-ci.com/Arasz/NaCoDoKina.Api)
|develop            |[![Develop Branch Build Status](https://travis-ci.com/Arasz/NaCoDoKina.Api.svg?token=jsB52VbhD4YcywUiDpoV&branch=develop)](https://travis-ci.com/Arasz/NaCoDoKina.Api)


# Documentation

API

.../shows (GET)
fetches all shows in given order (depending on user preferences and localization or just localization)
path params:
long (double)
lat (double)
userId

RESPONSE:

{
  "shows": [
    “id1”, “id2”, “id3”, “id4”, “id5”
  ]
}

odpowiedź gdy coś pójdzie nie tak:
{
“error”:{
		“code” : 123,
		“message” : “what a terrible failure” } 
}
.../show/{id} (GET)
returns general info about the show to display on the main page
BODY:
{
“screen”:”xhdpi”,
}
RESPONSE:
{
	 “title”: “show title”,
	 “imageUlr”: “https://goo.gl/2o3d9U”,
	 “rating”:7.8
}



.../show/{id} (DELETE)
do not return this show again (only if user logged in)
body:
{
“userId” : “1234563243”
}
response: 200
{ } 

.../show/{id}/details (GET)
fetches detailed info about the show

.../show/{id}/cinemas (GET)
fetches list of nearest cinemas which plays this movie and about show times from given cinema




