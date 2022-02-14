# holodemo

Augmented Reality project with Hololens 1 and Unity.

Containes 2 examples that make use of the following features of the Hololens:

- Spatial mapping

- Speech recognition

- Vuforia marker tracking

- Hand tracking

The examples are:

- EvalGame: A line with shape of a spiral is located in the room. With the hand
  or with a "Wand", it is possible to draw a line. The average distance between
  both lines will be displayed. Voice commands are "next target" and "move
  target" to change and move the target line and "start line" and "stop line" to
  start and stop drawing.

- HuntGame: The room is mapped in the background, and invisible ``Prize``
  objects (coins) are located in random vertices of the room. With the hand or
  with a "Wand" one has to search for the coins, the only clue is that particles
  that fly from the hand/wand are attracted by the invisible coin. To make it
  easier, it is possible to say "help me" and particles will fly directly to the
  hidden coin temporairly.

They can be chosen when the app starts by saying "draw lines" or "start game".
The "Wand" is an image marker from the Vuforia library that we put in a pencil.

Also serves as example of the following Unity features:

- LineRenderer

- TextMesh

- ParticleSystem and attractors

- Coroutines

[This is a report with more explanations, especially about the EvalGame
example](https://github.com/martinber/holodemo/blob/master/docs/EvalGame_report.pdf).

Screenshots of EvalGame:

![EvalGame screenshot](./docs/EvalGame_1.jpg)

![EvalGame screenshot](./docs/EvalGame_2.jpg)

![EvalGame screenshot](./docs/EvalGame_2.jpg)

Screenshots of HuntGame:

![HuntGame screenshot](./docs/HuntGame_1.jpg)

![HuntGame screenshot](./docs/HuntGame_2.jpg)
