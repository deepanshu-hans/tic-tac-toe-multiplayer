
### FOR PYTHON
__Note: This Project was built in Python 3.8.10__

1. Navigate to folder **"Flask App"**.
2. Install all python requirements from the ``requirements.txt`` file using ``pip install -r requirements.txt``.
3. Run the python file ``main.py``.
4. The Flask Server is up and ready, and the locally accessible URL would be visible on the console. ``Eg. 192.X.X.X:9000``.
5. Copy the URL link.


### FOR .NET
__Note: This Project uses NetCoreApp3.1__

1. Go-to the .NET Project directory for **"Player 1"** and follow below steps.
    - Open ``Program.cs`` and change the variable ``public static string URL = 192.X.X.X:9000`` to the copied URL address from the Python execution.
    - Open terminal and navigate to the **"Player 1"** folder and run the below command to build the console app based on your Operating System.
        > Universal: ``dotnet restore``
        > For Windows: ``dotnet publish -c Release -r win10-x64``
        > For Linux: ``dotnet publish -c Release -r ubuntu.16.10-x64``
        > For MacOS: ``dotnet publish -c Release osx.10.11-x64``
    - You'll find the app ``Player 1.<your-os-executable>`` in ``bin/debug/netcoreapp3.1/<your-os>/``

2. Repeat the same steps for project **Player 2**.
3. You'll now end up with ``Player 1.<your-os-executable>`` and ``Player 2.<your-os-executable>``.
4. Run ``Player 1.<your-os-executable>`` and ``Player 2.<your-os-executable>`` to play the game.