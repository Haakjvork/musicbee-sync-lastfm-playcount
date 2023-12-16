# musicbee-sync-lastfm-playcount
Synchronizes MusicBee playcount with LastFm data.
It uses the API endpoint **track.getInfo** to get the user playcount.
It automatically normalizes song titles and splits multiple artists, so it may make several queries per song.

# Options

**Last.fm username**: The Last.Fm username. **Requiered** to do the query.
**Query Album Artist too when different from Track Artist.** Optional. It will repeat all queries with the field AlbumArtist if it's different from Artist.
**Query Sort Title too when different from Track Name.** Optional. It will repeat all queries with the field SortTitle if it's different from SongTitle.

## Example about normalization

If we have a song like this:
SongTitle = Soy una fracasada estúpida (Feat. Mujeres)
SortTitle = Soy una fracasada estúpida
Artist = Aiko El Grupo;Mujeres
AlbumArtist = Aiko El Grupo

It will query track.getInfo several times. Becuase the accent (ú) is different from the original spanish accent (ú) and because the artist is splitted with a ;, it will do the following 6 queries instead of just 1:

1. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Aiko El Grupo;Mujeres"
2. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Aiko El Grupo"
3. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Mujeres"
4. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Aiko El Grupo;Mujeres"
5. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Aiko El Grupo"
6. Song: "Soy una fracasada estúpida (Feat. Mujeres)" Artist: "Mujeres"

If you also have enabled "Query Album Artist too when different from Track Artist", it would make another 2 queries using AlbumArtist, but as "Aiko El Grupo" was already queried, it won't happen in this example.

Also, if you have enabled "Query Sort Title too when different from Track Name", it will make 6 mores queries in this case, using SortTitle instead of SongTitle:

7. Song: "Soy una fracasada estúpida" Artist: "Aiko El Grupo;Mujeres"
8. Song: "Soy una fracasada estúpida" Artist: "Aiko El Grupo"
9. Song: "Soy una fracasada estúpida" Artist: "Mujeres"
10. Song: "Soy una fracasada estúpida" Artist: "Aiko El Grupo;Mujeres"
11. Song: "Soy una fracasada estúpida" Artist: "Aiko El Grupo"
12. Song: "Soy una fracasada estúpida" Artist: "Mujeres"

The sum of these 12 queries will be the new playcount.

**If the playcount is equal or less than 1 (usually because the track does not exist in Last.Fm) it WON'T change the playcount** so no changes would be make.


# LastFM API
The API key and secret are not included in the code.


# Dependencies

https://github.com/inflatablefriends/lastfm

Newtonsoft.Json 9.0.1 =<

System.Net.Http 4.3.0 =<
