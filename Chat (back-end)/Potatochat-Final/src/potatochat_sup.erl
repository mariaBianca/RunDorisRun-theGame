%
% Source code in Erlang use to supervise the worker for the chat .
% @author Mayra Soliz/m65v
% DIT029 H16 Project: Software Architecture for Distributed Systems
% University of Gothenburg, Sweden 2016
%
% Client functions, this are functions that a client can send to the server
% The Supervisor starts the link  
% with the superviser we creating a fault tolerant system
-module(potatochat_sup).
-behaviour(supervisor).
% export for the potatochat_sup
-export([
    start_link/0
]).
% export for the gen_server which contains the init
-export([
    init/1
]).

% Initializing the link with the supervisor 
% initializing the potatochat_serv and the worker
start_link() -> supervisor:start_link({global, ?MODULE}, ?MODULE, []).

init([]) ->
    Flags = {one_for_one, 0, 1}, % restart strategy a one_for_one,if one goes down 
    %only that one is restarted, I did not use the max restart limit I did not 
    %consideredn necesary 
    Children = [
    % Using util to state the specifications
        util:child(potatochat_serv, worker)
    ],
    % sending the flag and the list of specification from Util
    {ok, {Flags, Children}}.
