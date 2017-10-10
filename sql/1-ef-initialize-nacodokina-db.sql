CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "CinemaNetworks" (
        "Id" bigserial NOT NULL,
        "CinemaNetworkUrl" text NULL,
        "Name" varchar(255) NOT NULL,
        CONSTRAINT "PK_CinemaNetworks" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "DisabledMovies" (
        "MovieId" int8 NOT NULL,
        "UserId" int8 NOT NULL,
        CONSTRAINT "PK_DisabledMovies" PRIMARY KEY ("MovieId", "UserId")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "Movies" (
        "Id" bigserial NOT NULL,
        "PosterUrl" varchar(200) NOT NULL,
        "Title" varchar(80) NOT NULL,
        "AgeLimit" varchar(100) NOT NULL,
        "CrewDescription" varchar(300) NULL,
        "Description" text NOT NULL,
        "Director" varchar(100) NULL,
        "Genre" varchar(100) NOT NULL,
        "Language" varchar(100) NULL DEFAULT '',
        "Length" interval NOT NULL,
        "OriginalTitle" varchar(80) NOT NULL,
        "Production" text NULL,
        "ReleaseDate" timestamp NOT NULL,
        "MovieDetails_Title" varchar(80) NOT NULL,
        CONSTRAINT "PK_Movies" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "Cinemas" (
        "Id" bigserial NOT NULL,
        "Address" varchar(80) NOT NULL,
        "CinemaNetworkId" int8 NULL,
        "CinemaUrl" text NULL,
        "ExternalId" text NULL,
        "GroupId" text NULL,
        "Name" varchar(80) NOT NULL,
        "Location_Latitude" float8 NOT NULL,
        "Location_Longitude" float8 NOT NULL,
        CONSTRAINT "PK_Cinemas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Cinemas_CinemaNetworks_CinemaNetworkId" FOREIGN KEY ("CinemaNetworkId") REFERENCES "CinemaNetworks" ("Id") ON DELETE NO ACTION
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "ExternalMovie" (
        "Id" bigserial NOT NULL,
        "CinemaNetworkId" int8 NULL,
        "ExternalId" text NULL,
        "MovieId" int8 NOT NULL,
        "MovieUrl" text NULL,
        CONSTRAINT "PK_ExternalMovie" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ExternalMovie_CinemaNetworks_CinemaNetworkId" FOREIGN KEY ("CinemaNetworkId") REFERENCES "CinemaNetworks" ("Id") ON DELETE NO ACTION,
        CONSTRAINT "FK_ExternalMovie_Movies_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "MediaLink" (
        "Id" bigserial NOT NULL,
        "MediaType" int4 NOT NULL,
        "MovieDetailsId" int8 NULL,
        "Url" varchar(300) NOT NULL,
        CONSTRAINT "PK_MediaLink" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_MediaLink_Movies_MovieDetailsId" FOREIGN KEY ("MovieDetailsId") REFERENCES "Movies" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "ReviewLink" (
        "Id" bigserial NOT NULL,
        "LogoUrl" varchar(300) NULL,
        "MovieDetailsId" int8 NULL,
        "Name" varchar(50) NOT NULL,
        "Rating" float8 NOT NULL DEFAULT 0,
        "Url" varchar(300) NOT NULL,
        CONSTRAINT "PK_ReviewLink" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ReviewLink_Movies_MovieDetailsId" FOREIGN KEY ("MovieDetailsId") REFERENCES "Movies" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE TABLE "MovieShowtimes" (
        "Id" bigserial NOT NULL,
        "Available" bool NOT NULL,
        "BookingLink" text NULL,
        "CinemaId" int8 NOT NULL,
        "Language" varchar(100) NOT NULL,
        "MovieId" int8 NOT NULL,
        "ShowTime" timestamp NOT NULL,
        "ShowType" varchar(100) NOT NULL,
        CONSTRAINT "PK_MovieShowtimes" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_MovieShowtimes_Cinemas_CinemaId" FOREIGN KEY ("CinemaId") REFERENCES "Cinemas" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_MovieShowtimes_Movies_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE UNIQUE INDEX "IX_CinemaNetworks_Name" ON "CinemaNetworks" ("Name");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_Cinemas_CinemaNetworkId" ON "Cinemas" ("CinemaNetworkId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE UNIQUE INDEX "IX_Cinemas_Name" ON "Cinemas" ("Name");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_ExternalMovie_CinemaNetworkId" ON "ExternalMovie" ("CinemaNetworkId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_ExternalMovie_MovieId" ON "ExternalMovie" ("MovieId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_MediaLink_MovieDetailsId" ON "MediaLink" ("MovieDetailsId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE UNIQUE INDEX "IX_MediaLink_Url" ON "MediaLink" ("Url");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_MovieShowtimes_CinemaId" ON "MovieShowtimes" ("CinemaId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_MovieShowtimes_MovieId" ON "MovieShowtimes" ("MovieId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_ReviewLink_MovieDetailsId" ON "ReviewLink" ("MovieDetailsId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE INDEX "IX_ReviewLink_Name" ON "ReviewLink" ("Name");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    CREATE UNIQUE INDEX "IX_ReviewLink_Url" ON "ReviewLink" ("Url");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20171009202314_Intialization') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20171009202314_Intialization', '2.0.0-rtm-26452');
    END IF;
END $$;
