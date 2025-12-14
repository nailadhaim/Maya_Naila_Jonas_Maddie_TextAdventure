TextAdventure Projectoverzicht
Projectstructuur

Deze solution bestaat uit drie projecten:
Maya_Naila_Jonas_Maddie_TextAdventure (Client)
De console-applicatie waarin het text adventure spel gespeeld wordt.
TextAdventureApi (API)
Een Minimal API die verantwoordelijk is voor authenticatie, autorisatie, het beheren van JWT-tokens en het leveren van keyshares voor versleutelde kamers.
TextAdventure.Tests
Unit- en integratietests voor de game-logica van TextAdventure.

Security
Authenticatie
Gebruikers loggen in via de API met een gebruikersnaam en wachtwoord.
Wachtwoorden worden gehasht met SHA-256.
Na succesvolle login ontvangt de client een JWT-token.

Autorisatie
JWT bevat rol-informatie (Player / Admin).
De API controleert rollen via claims.
Admin-gebruikers hebben extra rechten.
Versleutelde kamers
Twee kamers zijn opgeslagen als .enc bestanden.
De inhoud is versleuteld en niet leesbaar zonder decryptiesleutel.
Keyshares worden veilig opgehaald via de API.

Secure coding
Inputvalidatie is aanwezig in zowel de client als de API.
Geen secrets zijn hardcoded in de code.

Programma starten
Start eerst de API
Open het project TextAdventureApi in Visual Studio.
Start het project.
Swagger-documentatie is beschikbaar via /swagger.
Start daarna de client
Open het project Maya_Naila_Jonas_Maddie_TextAdventure.
Start de console-applicatie.
Log in met een bestaande gebruiker.
Na login start het spel automatisch.

Testing
Het project is getest via een combinatie van unit tests, integratietesten en handmatige tests.
