/**
*Source code use to for the chat
*@author Mayra Soliz/m65v
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

-module(util).
-export([
    child/2,
    child/3,
    child/4
]).

child(Module, Type) -> child(Module, [Module], Type, permanent).

child(Module, Type, Restart) -> child(Module, [Module], Type, Restart).

child(Module, Modules, Type, Restart) ->
    #{
        id => Module,
        start => {Module, start_link, []},
        restart => Restart,
        shutdown => infinity,
        type => Type,
        modules => Modules
    }.
