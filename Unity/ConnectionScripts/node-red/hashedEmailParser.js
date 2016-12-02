if (msg.payload.indexOf("hashedEmail") !== -1){
    msg.payload = JSON.parse(msg.payload);
    msg.projection = JSON.parse('{"_id":0, "name":0, "user_ID":0, "hashedPass":0, "typeOfClient":0}');
    msg.limit = 1;
    msg.sort = {"hashedEmail":-1};
    msg.skip = 0;
}

return msg;