/**
*Source Code in Erlang to call the functions of the chat.
*@author Mayra Soliz/m65v
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
-module(potatochat_app).
-behaviour(application).
-export([
    start/2,
    stop/1
]).

start(_StartType, _StartArgs) -> potatochat_sup:start_link().
stop(_State) -> ok.
