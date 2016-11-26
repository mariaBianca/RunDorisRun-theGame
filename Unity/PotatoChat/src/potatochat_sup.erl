/**
*Source code in Erlang use to supervise the worker for the chat .
*@author Mayra Soliz/m65v
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
-module(potatochat_sup).
-behaviour(supervisor).
-export([
    start_link/0
]).
-export([
    init/1
]).

start_link() -> supervisor:start_link({local, ?MODULE}, ?MODULE, []).

init([]) ->
    Flags = {one_for_one, 0, 1},
    Children = [
        util:child(potatochat_serv, worker)
    ],
    {ok, {Flags, Children}}.
