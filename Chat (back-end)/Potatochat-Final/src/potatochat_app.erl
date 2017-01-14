%
%Source Code in Erlang to call the functions of the chat.
%@author Mayra Soliz/m65v
%DIT029 H16 Project: Software Architecture for Distributed Systems
%University of Gothenburg, Sweden 2016
%
% The aplication behavoir allow the functionality us 
% to start and stop processes(modules). This process will start 
% at the erlang run time system. I can also access application information 
% The potatochat_app supervices the potatochat_sup which supervices the server
%
-module(potatochat_app).
-behaviour(application).
-export([
    start/2,
    stop/1
]).
%%The start the application that call the top level supervisor  

start(_StartType, _StartArgs) -> potatochat_sup:start_link().

%%The stop function will stop the chat 

stop(_State) -> ok.

% I use this one to make the application resource file, it will have a diferent 
% extention called app 