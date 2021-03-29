const request = require('request')

module.exports = {
    make_API_call : function(url){
        return new Promise((resolve, reject) => {
            request(url, { json: true }, (err, res, body) => {
                // console.log(body);
              if (err) reject(err)
              resolve(body)
            });
        })
    }
}