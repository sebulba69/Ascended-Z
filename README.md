Ascended Z v0.04

This is a pre-alpha build of Ascended Z whose goal is to nail down the game's core loop.

Goal: Reach the top floor. The current top floor of this build is Floor 100 of the main game and Floor 100 of the Labrybuce (total: 200 floors).

Controls: Mouse (Menus, combat, etc.), Arrow Keys (Combat, Dungeon Crawling), Z (Select) and X (Back) (Combat, Dungeon Crawling only).

~ How to Play: Combat ~

Ascended Z is a combat focused RPG using Press Turn as it's main battle system. Press Turn is a combat system that focuses on changing the number of turns players and enemies have based on their actions in combat.

Here are all actions in battle and how they affect the active battler's turn:
1. Basic action ([None])- By default, 1 turn icon is consumed per action
2. Weaknesses (Wk) - Only uses half a turn, preserving it for the next battler in the turn order
3. Technicals - Behave similarly to a weakness
4. Resist (Rs) - Incoming damage is reduced, and 1 turn icon is consumed like normal
5. Void (Nu) - 2 turn icons are used and no damage is dealt. If only 1 turn icon remains, then the active battler's turn ends
6. Drain (Dr) - All turn icons are consumed and the target of the skill gains the damage they would have taken as HP
7. Passing - Turn the current full icon into a half icon and pass the current action onto the next battler in the turn order
8. Guard - Turns any non-Void/Drain attack into a Resist. Consumes 1 full icon to use. Your guard is removed if you choose to do any other action (except Passing) after using it.

Important notes:
* Basic Actions will always use up the first available half icon before using up a full icon
* Voids will consume half icons in the same way
* Repeated weakness hits will turn all available full icons into half icons until all full icons are halved. Afterward, half icons will be fully used up when no new ones can be created
* If multiple actions are triggered at once, they are treated according to this priority: 1. Drains, 2. Voids, 3. Weaknesses, 4. Resists/Normals. If a Wk and a Dr are hit at the same time, the Dr will take priority over the Wk and deplete all turn icons. If a Wk is hit as well as a Rs/[None], then the Wk will take priority over the Rs/[None]

In battle, all enemies have specific AI that determines their actions. Hover over an enemy's picture to read what their AI can do. This does not apply to bosses however, as they all follow their own individual scripts.



~ How to Play: Currency ~

There are 6 main currencies in Ascended Z:
* Vorpex - Used for levelling up characters
* Party Coin - Used for buying/fusing characters
* Dellencoin - Used for levelling up the shop
* Key Shards - When 4 are collected, a Bounty Key is formed
* Bounty Key - Used to fight Bounty Bosses (super bosses)  -- currently not implemented
* Morbis - Used during dungeon crawling to purchase pickaxes



~ How to Play: Recruiting/Fusing ~

Based on your stage in the game, you can recruit 2 different types of party members: Normal, and Custom. Custom party members let you customize their skills. The only limit they have is that you can't give them a skill that they'd be weak to.

You can fuse party members that have the same "primary resistance" as one another. A "primary resistance" is the resistance with the highest value. For example, if I have a character with the resistances [Rs: Fire, Wk: Ice] I can fuse with any other party member whose resistances are [Dr/Nu/Rs] Fire.

Fusions also have what's called a "Fusion Grade." By default, all Fusion Grades are 0. You can only fuse party members together who either have the same Fusion Grade or a difference in Fusion Grades of 1.

The last rule with Fusions is that you cannot have duplicates.



~ How to Play: Upgrading ~

The Upgrade Screen allows you to level up party member skills or refund party members for upgrade items. Upgrading is self explanatory.
To refund, click the Refund button above the party member's portrait. When done, you will get back the Vorpex and Party Coin pictured to the right of the button.

~ How to Play: Labrybuce ~

The Labrybuce is the Dungeon Crawling portion of the game. You can access it at Tier 10 of the main game by clicking the "Labrybuce" button in the Embark Screen. Like the main game, you also have a tier that goes up as you complete floors. To complete a floor, you need to find the exit stairs and defeat the number of "Required Encounters" displayed on the left hand side of the screen.

Dungeons consist of the following events:
* Items (Brown Chest) - Randomized set of items (Dellencoin, Vorpex, or Party Coin)
* Special Items (White Chest) - Guaranteed set of items (Dellencoin, Vorpex, and Party Coin)
* Encounters (White Skull) - Basic encounters from the main game
* Special Encounters (Purple Skull) - Randomized AI w/ randomized skills
* Special Boss Encounters (Gold Door) - Randomized bosses with randomized skills, completes all Encounters for the floor when defeated
* Orbs (Red Orb) - Currency for the Buce Fountain
* Buce Fountain (Fountain) - Spend 3 orbs to get 1 Morbis
* Miner - Spend 1 Morbis to get 3 Pickaxes. Pickaxes can be used to create tiles in the dungeon where tiles are missing. This can be useful for making your own shortcuts when the maze doesn't allow you to. Just click on one of the 4 available buttons surrounding your player character to use 1 pickaxe to dig out new path.
* Pot of Greed: Collect 1 Key Shard or, if that 1 Key Shard makes 4, remove your other key shards and replace them with 1 Bounty Key instead
