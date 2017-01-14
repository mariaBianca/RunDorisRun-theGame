#!/bin/bash

EBIN="ebin/ deps/*/ebin/"
EVAL="application:ensure_all_started(potatochat)"

rebar compile
erl -pa $EBIN -eval $EVAL
