from flask import request, jsonify

import flask, json, requests, os, socket

app = flask.Flask(__name__)

@app.route('/', methods=['GET'])
def index():
    try:
        return os.environ['SOURCE_VERSION']
    except:
        return socket.gethostname()

@app.route('/dapr/subscribe', methods=['GET'])
def subscribe():
    subscriptions = [
                        {'pubsubname': 'pubsub', 'topic': 'consultasrejeitadas', 'route': 'consultasrejeitadas'},
                        {'pubsubname': 'pubsub', 'topic': 'consultasagendadas', 'route': 'consultasagendadas'}
                    ]
    return jsonify(subscriptions)

@app.route('/consultasrejeitadas', methods=['POST'])
def consultasrejeitadas_subscriber():
    sendmessage(request.json['data'])
    return json.dumps({'success':True}), 200, {'ContentType':'application/json'}

@app.route('/consultasagendadas', methods=['POST'])
def consultasagendadas_subscriber():
    sendmessage(request.json['data'])
    return json.dumps({'success':True}), 200, {'ContentType':'application/json'} 

def sendmessage(payload):
    dapr_url = "http://localhost:{}/v1.0/bindings/sendmail".format(3602)
    post_data = { "metadata": payload['metadata'], "data": payload['data'], "operation": "create" }
    response = requests.post(dapr_url, json=post_data)

app.run(host='0.0.0.0', port=7000)