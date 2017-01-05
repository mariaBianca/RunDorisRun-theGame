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
%%The start function that call the function %%potato_sup:start_link  

start(_StartType, _StartArgs) -> potatochat_sup:start_link().

%%The stop function will stop the chat 

stop(_State) -> ok.
