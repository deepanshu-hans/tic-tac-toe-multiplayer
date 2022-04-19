from http.client import responses
from flask import Flask, request
from flask_sqlalchemy import SQLAlchemy
from flask import jsonify
from play_game import play
from enums import tick_O, tick_X

db_name = 'tic_tac_toe'
app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///'+db_name+'.sqlite3'
app.config['SECRET_KEY'] = "randomstring"

valid_tick_types = [tick_X.lower(), tick_O.lower()]
valid_index_range = list(range(10))

db = SQLAlchemy(app)
responses = ["Game running!", 1]

class TTDB(db.Model):
   _id = db.Column(db_name, db.Integer, primary_key = True)
   p_name = db.Column(db.String(100))
   tick_type = db.Column(db.String(100))
   index_pos = db.Column(db.String(100)) 
   
   def __init__(self, p_name, tick_type, index_pos):
       self.p_name = p_name
       self.tick_type = tick_type
       self.index_pos = index_pos

def delete_db():
    global responses
    responses = ["Game running!", 1]
    db.drop_all()
    db.create_all()

def validate_content(content):

    if (content['tick_type'].lower() not in valid_tick_types) or (int(content['index_pos']) not in valid_index_range):
        return False
    else:
        return True

@app.route('/results/')
def show_result():
    return jsonify(
            response = responses[0],
            game = responses[1]
    )

@app.route('/clear_game/')
def clear_game():
    delete_db()
    return jsonify(
            response = "Previous Game Cleared!"
    )


@app.route('/play', methods = ['GET', 'POST'])
def new():
    global responses
    if request.method == 'POST':
        content = request.json

        if not validate_content(content):
            return jsonify(
            flag = 1,
            response= "Invalid Choice! Please try again."
        )

        all_data = TTDB.query.all()
        if len([int(instance.index_pos) for instance in all_data if int(instance.index_pos) != 0]) >= 9:
            delete_db()
            responses = ["Game restarted...", 0]
            return jsonify(
            flag = 0,
            response= "Game restarted..."
        )

        if int(content['index_pos']) in [int(instance.index_pos) for instance in all_data if int(instance.index_pos) != 0]:
            return jsonify(
            flag = 1,
            response= "Position already taken!, Try Another one."
        )

        current_move = TTDB(content['p_name'], content['tick_type'], content['index_pos'])
        db.session.add(current_move)
        db.session.commit()
        
        if content['index_pos'] != 0:
            play_response = play(TTDB.query.all())
            if play_response[1]:
                responses = [play_response[0], 0]
                return jsonify(
                    flag = 0,
                    response = play_response[0]
                )
            else:
                return jsonify(
                    flag = 1,
                    response = "Next move..."
                )
        else:
            return jsonify(
                    flag = 1,
                    response = "Passed move..."
                )
    else:
        data = [
            { 
                "name": instance.p_name, 
                "index_pos": instance.index_pos,
                "tick_type": instance.tick_type
            } for instance in TTDB.query.all()
        ]
        return jsonify(
            response = data
        )

if __name__ == '__main__':
   db.create_all()
   app.run(host='0.0.0.0', port=9000, debug=True)