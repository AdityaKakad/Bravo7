const express = require('express')
const server = express();
const api_helper = require('./API_helper')
const cors = require('cors');
const bodyParser = require('body-parser');

// TODO: Do data processing here before sending
server.use(cors())
server.use(bodyParser.json());

const url = "http://dreamlo.com/lb/";
const privateCode = "Gf9B1vbb1kGsPKcpH53qOwJ4QLSY7f606gYxdahmaN1w";
const publicCode = "6060fb7f8f421366b0545c75";

//Adding routes
server.get('/', (request, response) => {
    response.sendFile(__dirname + '/src/index.html');
});

server.get('/send', (request, response) => {
    let name = request.query.name;
    let score = request.query.score;

    let webURL = url + privateCode + "/add/" + name + "/" + score;
    response.setHeader("Access-Control-Allow-Origin", "*");
    response.setHeader("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS")
    response.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    api_helper.make_API_call(webURL)
            .then((res) => {
                response.sendStatus(200);
            })
            .catch(error => {
                response.send(error);
            })  
});

server.get('/receive', (request, response) => {
    let webURL = url + publicCode + "/pipe/";
    response.setHeader("Access-Control-Allow-Origin", "*");
    response.setHeader("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS")
    response.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    api_helper.make_API_call(webURL)
            .then((res) => {
                response.send(res);
            })
            .catch(error => {
                response.send(error);
            })  
});

//Express error handling middleware
server.use((request, response) => {
    response.type('text/plain');
    response.status(505);
    response.send('Error page');
});

// Start the server
const PORT = process.env.PORT || 8080;
server.listen(PORT, () => {
    console.log(`App listening on port ${PORT}`);
    console.log('Press Ctrl+C to quit.');
});

module.exports = server;
