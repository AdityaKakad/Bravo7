const express = require('express')
const server = express();
const api_helper = require('./API_helper')
const cors = require('cors');

// TODO: Do data processing here before sending
server.use(cors())

//setting the port.
server.set('port', process.env.PORT || 5000);
var guardianKey = "a35404af-183b-443d-b839-92b6505bed04";
var NYtimesKey = "0WTyqQilAQjEAjeFI9rtZ22Z1YDNRup3";

//Adding routes
server.get('/', (request, response) => {
    response.sendFile(__dirname + '/index.html');
});

server.get('/home', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.setHeader("Access-Control-Allow-Origin", "*");
    response.setHeader("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS")
    response.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/home.json?api-key=" + NYtimesKey)
            .then((res) => {
                //New way
                let articles = res.results;
                let status = res.status;

                let formattedArticles = articles.map(art => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;

                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                });
                response.send({ formattedArticles, status });
            })
            .catch(error => {
                response.send(error);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/search?api-key=" + guardianKey + "&section=(sport|business|technology|politics)&show-blocks=all")
            .then((res) => {
                let articles = res.response.results;
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let title = art.webTitle;
                    let artId = art.id;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if(checkIfImageExists(art, newsChannel)){
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, imageUrl, type: 'Guardian'
                        }
                    } else  return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }  
                });
                response.send({ formattedArticles, status });
                // console.log(formattedArticles);
            })
            .catch(error => {
                response.send(error);
            })
    }
});

server.get('/world', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.setHeader("Access-Control-Allow-Origin", "*");
    response.setHeader("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS")
    response.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/world.json?api-key=" + NYtimesKey)
            .then((res) => {
                let articles = res.results;
                let status = res.status;
                // console.log(articles);

                let formattedArticles = articles.map((art) => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ 
                    formattedArticles,
                     status });
                // console.log(formattedArticles);
            })
            .catch(err => {
                response.send(err);
            });
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/world?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
                // console.log(formattedArticles);
            })
            .catch(err => {
                response.send(err);
            })
    }
});

server.get('/politics', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/politics.json?api-key=" + NYtimesKey)
            .then(res => {
                let articles = res.results;
                let status = res.status;

                let formattedArticles = articles.map(art => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ formattedArticles, status });
                // console.log(formattedArticles);
            })
            .catch(err => {
                response.send(err);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/politics?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(err => {
                response.send(err);
            })
    }
});

server.get('/business', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/business.json?api-key=" + NYtimesKey)
            .then(res => {
                let articles = res.results;
                let status = res.status;

                let formattedArticles = articles.map(art => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ formattedArticles, status });
                // console.log(formattedArticles);
            })
            .catch(err => {
                response.send(err);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/business?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(err => {
                response.send(err);
            })
    }
});

server.get('/technology', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/technology.json?api-key=" + NYtimesKey)
            .then(res => {
                let articles = res.results;
                let status = res.status;
                // console.log(res);

                let formattedArticles = articles.map(art => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ formattedArticles, status });
                // console.log(formattedArticles);
            })
            .catch(err => {
                response.send(err);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/technology?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(err => {
                response.send(err);
            })
    }
});

server.get('/sports', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/topstories/v2/sports.json?api-key=" + NYtimesKey)
            .then(res => {
                let articles = res.results;
                let status = res.status;
                // console.log(res);

                let formattedArticles = articles.map(art => {
                    let section = art.section.length>0?art.section:'other';
                    let title = art.title;
                    let pubDate = art.published_date;
                    let description = art.abstract;
                    let artUrl = art.url;
                    let authorLine = art.byline;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? images[0].url : 'default';
                    }
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(err => {
                response.send(err);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/sport?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(err => {
                response.send(err);
            })
    }
});


// For detailed article access
server.get('/article', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    let id = request.query.id;
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/search/v2/articlesearch.json?fq=web_url:(\"" + id + "\")&api-key=" + NYtimesKey)
            .then(res => {
                let status = res.status;
                let art = res.response.docs[0];
                // console.log(art);

                let section = art.news_desk.length>0?art.news_desk:'other';
                let title = art.headline.main;
                let pubDate = art.pub_date;
                let description = art.abstract;
                let artUrl = art.web_url;
                let authorLine = art.byline.original;
                let imageUrl = "default";
                if(art.multimedia){
                    let images = art.multimedia.filter((img) => {return img.width >= 2000});
                    imageUrl = images.length > 0 ? "https://www.nytimes.com/"+images[0].url : 'default';
                }
                // console.log(imageUrl);
                let formattedArticle = {
                    section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                }
                // console.log(formattedArticle);
                response.send({ formattedArticle, status });
            })
            .catch(error => {
                response.send(error);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/" + id + "?api-key=" + guardianKey + "&show-blocks=all")
            .then(res => {
                let art = res.response.content;
                let status = res.response.status;
                let formattedArticle = guardianDetailedArticle(art, newsChannel);
                // console.log(formattedArticle);
                response.send({ formattedArticle, status });
            })
            .catch(error => {
                response.send(error);
            })
    }
});

guardianDetailedArticle = (art, newsChannel) => {
    let section = art.sectionId.length>0?art.sectionId:'other';
    let title = art.webTitle;
    let artId = art.id;
    let pubDate = art.webPublicationDate;
    let description = '';
    if(art.blocks && art.blocks.body){
        description = art.blocks.body[0].bodyTextSummary;
    }
    let artUrl = art.webUrl;
    if(checkIfImageExists(art, newsChannel)){
        let assets = art.blocks.main.elements[0].assets;
        let imageUrl = assets[assets.length - 1].type == "image" ?
            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
            : 'default';
        return {
            artId, section, title, pubDate, description, artUrl, imageUrl, type: 'Guardian'
        }
    } else  return {
        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
    }  
}

server.get('/search', (request, response) => {
    let newsChannel = request.header('news-provider');
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    let kw = request.query.keyword;
    if (newsChannel == "NY") {
        api_helper.make_API_call("https://api.nytimes.com/svc/search/v2/articlesearch.json?q=" + kw + "&api-key=" + NYtimesKey)
            .then((res) => {
                let articles = res.response.docs;
                let status = res.status;
                // console.log(articles);

                let formattedArticles = articles.map(art => {
                    let section = art.news_desk.length>0?art.news_desk:'other';
                    let title = art.headline.main;
                    let pubDate = art.pub_date;
                    let description = art.abstract;
                    let artUrl = art.web_url;
                    let authorLine = art.byline.original;
                    let imageUrl = "default";
                    if(art.multimedia){
                        let images = art.multimedia.filter((img) => {return img.width >= 2000});
                        imageUrl = images.length > 0 ? "https://www.nytimes.com/"+images[0].url : 'default';
                    }
                    // console.log(imageUrl);
                    return {
                        section, title, pubDate, description, artUrl, authorLine, imageUrl, type: 'NY'
                    }
                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(error => {
                response.send(error);
            })
    } else {
        api_helper.make_API_call("https://content.guardianapis.com/search?q=" + kw + "&api-key=" + guardianKey + "&show-blocks=all")
            .then((res) => {
                let articles = res.response.results;
                // console.log(articles);
                let status = res.response.status;

                let formattedArticles = articles.map(art => {
                    let section = art.sectionId.length>0?art.sectionId:'other';
                    let artId = art.id;
                    let title = art.webTitle;
                    let pubDate = art.webPublicationDate;
                    let description = '';
                    if(art.blocks && art.blocks.body){
                        description = art.blocks.body[0].bodyTextSummary;
                    }
                    let artUrl = art.webUrl;
                    if (checkIfImageExists(art, newsChannel)) {
                        let assets = art.blocks.main.elements[0].assets;
                        let imageUrl = assets[assets.length - 1].type == "image" ?
                            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
                            : 'default';
                        return {
                            artId, section, title, pubDate, description, artUrl, type: 'Guardian', imageUrl
                        }

                    } else return {
                        artId, section, title, pubDate, description, artUrl, imageUrl: 'default', type: 'Guardian'
                    }

                }).slice(0, 10);
                response.send({ formattedArticles, status });
            })
            .catch(error => {
                response.send(error);
            })
    }
});

checkIfImageExists = (art, newsChannel) => {
    if (newsChannel == "NY") return true;
    try {
        let assets = art.blocks.main.elements[0].assets;
        if (assets == undefined || assets == null) return false;
        let imageUrl = assets[assets.length - 1].type == "image" ?
            (assets[assets.length - 1].file ? assets[assets.length - 1].file : 'default')
            : 'default';
        if (imageUrl == undefined || imageUrl == null) return false;
        return true;
    }
    catch (err) {
        return false;
    }
}

//Express error handling middleware
server.use((request, response) => {
    response.type('text/plain');
    response.status(505);
    response.send('Error page');
});

//Binding to localhost://3000
server.listen(8080, () => {
    console.log('Express server started at port 8080');
});