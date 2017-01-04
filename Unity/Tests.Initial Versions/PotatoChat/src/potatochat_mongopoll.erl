/**
*Source code use to stablish the connection between mongoDB
*and Erlang
*@author Mayra Soliz/m65v
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
-module(potatochat_mongopoll).
-export([
    start/0,
    init/1,
    loop/1,
    stop/1
]).

-define(MONGO_HOST, "localhost").
-define(MONGO_PORT, 27017).
-define(MONGO_DATABASE, <<"potatochat">>).
-define(MONGO_COLLECTION, <<"message">>).

-define(PUB_FLAG, <<"pub">>).
-define(PUB_INTERVAL, 100).

start() ->
    Pid = proc_lib:spawn_link(?MODULE, init, [{replyTo, self()}]),
    {ok, Pid}.

init({replyTo, ReplyTo}) ->
    {ok, Mongo} = mc_worker_api:connect([{host, ?MONGO_HOST},
                                         {port, ?MONGO_PORT},
                                         {database, ?MONGO_DATABASE}]),
    loop({state, ReplyTo, Mongo}).

loop({state, ReplyTo, Mongo}) ->
    receive
        stop -> ok
    after
        ?PUB_INTERVAL ->
            Cursor = mc_worker_api:find(Mongo, ?MONGO_COLLECTION,
                                        {?PUB_FLAG, {<<"$exists">>, false}}),
            Result = mc_cursor:rest(Cursor),
            mc_cursor:close(Cursor),

            Send = fun(Document) ->
                #{<<"_id">> := DocumentId,
                  <<"text">> := DocumentText} = Document,

                ReplyTo ! {publish, jiffy:encode({[{text, DocumentText}]})},

                Command = {<<"$set">>, {?PUB_FLAG, true}},
                mc_worker_api:update(Mongo, ?MONGO_COLLECTION,
                                     {<<"_id">>, DocumentId},
                                     Command)
            end,

            [Send(Document) || Document <- Result],

            loop({state, ReplyTo, Mongo})
    end.

stop(Pid) -> Pid ! stop.
