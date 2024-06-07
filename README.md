# ContainerShip algorithm

This project is an algorithm for placing containers on a ship.
The algorithm adheres to the following requirements:

- The max weight on top of a container is 120t
- The max weight of one container is 30t
- An empty container weighs 4t
- A valuable container must be on the top of the stack
- A valuable container must be accessible from either the front or the back
- Every coolable container must be in the first row
- At least 50% of the max weight of the ship must be filled
- The difference between left and right must be below 20% or the total load
- The size of the ship must be configurable in length and width

These requirements are checked by unit tests found in the test project.
The application has no gui, because that would be too much work.
Running the unit tests shows that the application confirms to all of the requirements.
