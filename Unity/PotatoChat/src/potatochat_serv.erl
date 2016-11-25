-module(potatochat_serv).
-behaviour(gen_server).
-export([
    start_link/0
]).
-export([
    init/1,
    handle_call/3,
    handle_cast/2,
    handle_info/2,
    terminate/2,
    code_change/3
]).

-define(MQTT_HOST, "localhost").
-define(MQTT_CLIENT, <<"potatochat">>).
-define(MQTT_TOPIC, <<"potatochat/message">>).

start_link() -> gen_server:start_link(?MODULE, default, []).

init(default) ->
    {ok, MQTT} = emqttc:start_link([{host, ?MQTT_HOST},
                                    {client_id, ?MQTT_CLIENT},
                                    {logger, info}]),

    {ok, MongoPoll} = potatochat_mongopoll:start(),

    {ok, {state, MQTT, MongoPoll}}.

handle_call(_Request, _From, State) -> {noreply, State}.

handle_cast(_Request, State) -> {noreply, State}.

handle_info({publish, Data}, {state, MQTT, MongoPoll}) ->
    emqttc:publish(MQTT, ?MQTT_TOPIC, Data),
    {noreply, {state, MQTT, MongoPoll}};

handle_info(_Message, State) -> {noreply, State}.

terminate(_Reason, {state, MQTT, MongoPoll}) ->
    potatochat_mongopoll:stop(MongoPoll),
    emqttc:disconnect(MQTT).

code_change(_OldVsn, State, _Extra) -> {ok, State}.
