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
