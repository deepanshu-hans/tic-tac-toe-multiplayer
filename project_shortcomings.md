### SHORTCOMINGS

Below mentioned are the few shortcomings of the project that I came accross. 

1. Symbol can be duplicated within **Player 1** and **Player 2**. Therefore a mutual decision is needed to be made within two players for selection of a symbol.
2. The whole project is developed and tested in **Linux** Environment using ``NetCoreSDK3.1`` and ``Python 3.8.10``. Runtime or dependency issues on other platforms aren't known.
3. The Flask app when deployed, it choses the URL dynamically. Which then is needed to be fed in the .NET applications.