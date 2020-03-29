# 999% Orange Juice
Simple Currency Cheat for 100% Orange Juice [Steam]. All it does is set each currency type to their maximum values so you can buy whatever you want from the store. 

Note that this program will only be able to modify the following currency values stored locally:
- Stars
- Oranges
- Halloween Candy

(Depreciated: Christmas Candy, Valentines Chocolates - Read [the wiki](https://100orangejuice.gamepedia.com/Currency) for more information. Any currencies listed under Steam Inventory cannot be modified by this program)

With the steam inventory upgrade, more currencies are being moved to the steam inventory, which cannot be modified by this program, and thus you will not be able to buy all the cosmetics. I believe that fruitbat factory will eventually move all currencies (apart from stars) to the inventory, which will eventually render this cheat useless.

### [Latest Release (24/07/19)](https://github.com/tsuneko/Orange-Juice/releases/download/2.2.1/999percent.zip)
### Last verified working 11/03/20 ~ 100% Orange Juice 2.8.2

To find the currencies, 999percent utilises signature scanning to find where the currencies are stored in memory. As 999percent has been updated to use signature scanning rather than static offsets, if the game is updated then the offsets will not become outdated as easily.

### Latest Signatures:
Signatures are listed on the conf.ini file and are in the form `name=array,mask,offset,max_value`
- Stars: `stars=FF75C0E800000000FF35000000008BB3,xxxx????xx????xx,10,99999`
- Oranges: `oranges=81FF000000007CCAA1000000005335,xx????xxx????xx,9,999`
- Halloween Candy: `halloween=E8000000000FB6C0330500000000536A,x????xx?xx????xx,10,999`

A tutorial on finding signatures can be found [here](https://www.unknowncheats.me/forum/programming-for-beginners/171994-understanding-pattern-scanning-concept.html).

### Disclaimer:

100% Orange Juice is a VAC enabled game. VAC requires an active internet connection to function, and so it is usually safe to cheat if the game is launched without an internet connection.

**Users have been banned using cheat engine in both singeplayer and multiplayer matches. Do not attempt to play a match with this cheat open.**

Fruitbat Factory has [mentioned](https://steamcommunity.com/app/282800/discussions/0/1744480966997301464/) that you may be banned even without being in a match, “There are no VAC servers, since the game doesn't use servers. The game itself is VAC enabled, meaning if you cheat while running it you're likely to get VAC banned.”, however I believe this statement is misleading as it conflicts with the design of VAC outlined [here](https://support.steampowered.com/kb_article.php?s=087dccfcc85be81977b078f1e4025bde&ref=7849-RADZ-6869).

Use entirely at own risk. I am not responsible for anything which can happen in regards to usage of my sources and/or releases.

As far as I am aware, this cheat will not result in a VAC ban as it only modifies currency values. There are no known cases of VAC bans due to this cheat.
