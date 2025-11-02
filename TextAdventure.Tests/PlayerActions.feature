Feature: Player actions in the dungeon
  As a player
  I want to explore the dungeon and interact with items
  So that I can win the game or lose if I make wrong choices

  Scenario: Pick up the sword and defeat the monster
    Given the player is in the start room
    When the player goes south
    And the player takes the sword
    And the player goes south again
    And the player fights the monster
    Then the monster should be defeated
    And the player should not be dead

  Scenario: Try to fight monster without sword
    Given the player is in the start room
    When the player goes south
    And the player goes south again
    And the player fights the monster
    Then the player should be dead

  Scenario: Try to open the locked door without key
    Given the player is in the start room
    When the player goes north
    Then the player should not be able to enter

  Scenario: Go in wrong direction
    Given the player is in the start room
    When the player goes east
    Then the player should see "You can't go that way"
