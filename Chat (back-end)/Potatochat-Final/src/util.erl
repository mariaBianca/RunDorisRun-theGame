%
% Source code use to for the chat
% @author Mayra Soliz/m65v
% DIT029 H16 Project: Software Architecture for Distributed Systems
% University of Gothenburg, Sweden 2016
%
% Part of the supervisor defining the child specification 
-module(util).
-export([
    child/2,
    child/3,
    child/4
]).

child(Module, Type) -> child(Module, [Module], Type, permanent). % always restart

child(Module, Type, Restart) -> child(Module, [Module], Type, Restart).

child(Module, Modules, Type, Restart) ->
    #{
        id => Module,
        start => {Module, start_link, []},
        restart => Restart,
        shutdown => infinity, %defines how the job procces should be terminated (cuould use "exit" or int timeout)
        type => Type,%specify is the type is superviser or a worker
        modules => Modules % is use by the release handler to determing which proces are using certain module
    }.
