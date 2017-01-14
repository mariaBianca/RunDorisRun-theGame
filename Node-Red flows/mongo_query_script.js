if(msg.payload=="{\"Level\":1}"){
    msg.payload = JSON.parse('{"Level_ID":1}');
    msg.projection = JSON.parse('{"_id":0}');
    msg.limit = 3;
    msg.sort = {"Score":-1};
    msg.skip = 0;
}
if(msg.payload=="{\"Level\":2}"){
    msg.payload = JSON.parse('{"Level_ID":2}');
    msg.projection = JSON.parse('{"_id":0}');
    msg.limit = 3;
    msg.sort = {"Score":-1};
    msg.skip = 0;
}
return msg;