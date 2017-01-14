%
%Source code that for the chat that connnects the
% server with mosquito.
%@author Mayra Soliz/m65v
%DIT029 H16 Project: Software Architecture for Distributed Systems
%University of Gothenburg, Sweden 2016
%
%the server connect to mongoDB, publicates the prata and it chage it to done false to true
% the message that is in pub(publicate) changing to true ones is publish

% gen server is a behavoiral module, is a good model for implementing a simple client server 
% relationship

% gen server is an abstracction over a typical process message loop


%Using behaviour (genericpart) is basically a way for a module to specify functions
% it expects another function to have
%
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
%defining macros 
-define(MQTT_HOST, "129.16.155.34").
-define(MQTT_CLIENT, <<"potatochat">>).
-define(MQTT_TOPIC, <<"potatochat/message">>).

start_link() -> gen_server:start_link(?MODULE, default, []).
% declaring the call back functions
% Initializng the server state and do all the others one time tasks
% that it will depend on the function will return 
init(default) ->
    {ok, MQTT} = emqttc:start_link([{host, ?MQTT_HOST},
                                    {client_id, ?MQTT_CLIENT},
                                    {logger, info}]),

    {ok, MongoPoll} = potatochat_mongopoll:start(),

    {ok, {state, MQTT, MongoPoll}}.


% works with syncronous messages, it basically will call the potato logic
% and it will wait for reply in this case is not replaying 
% then it will go to handle_info
%wherever is in the reply will be send back to whowver called the server 
% in the first place
% by unsing no reply the generic part of the server is asummng that 
% I am taking care of sending the reply my self
handle_call(_Request, _From, State) -> {noreply, State}.

% The hansde_cast handles assincronouis calls only tuples with no reply 
% are valid

handle_cast(_Request, State) -> {noreply, State}.

% since the handle_call does not have any reply the handle info 
% comes in acction, after  the handle_call the handle info will 
% be called by the server
% publish data to mqtt, ones that do that it change it to false
% 

handle_info({publish, Data}, {state, MQTT, MongoPoll}) ->
    emqttc:publish(MQTT, ?MQTT_TOPIC, Data),
    {noreply, {state, MQTT, MongoPoll}};

% is called by the gen server when the timeout occurs again since 
% I dont have any timeout

handle_info(_Message, State) -> {noreply, State}.

% is called wherever one of the other handlers returns a tuple of the form 
% is taking to parameters reason and state
% it will be call when its parent (process that spwan it) dies but in this case
% the server is not trapping exits 
% it will stop the connection with mongopolland emqttc
terminate(_Reason, {state, MQTT, MongoPoll}) ->
    potatochat_mongopoll:stop(MongoPoll),
    emqttc:disconnect(MQTT).

% The state holds the current server state to convert it 
% e.g. imagen that it bacomes to slow the convertion from one data 
%structure to another can be done there, just declaring not using 
code_change(_OldVsn, State, _Extra) -> {ok, State}.
