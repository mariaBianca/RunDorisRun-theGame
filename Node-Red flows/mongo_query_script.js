/**
@author TheHub
DIT029 H16 Project: Software Architecture for Distributed Systems
University of Gothenburg, Sweden 2016
This file is part of "Run Doris Run!" game.
"Run Doris Run!" game is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Run Doris Runis distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with "Run Doris Run!" game.  If not, see <http://www.gnu.org/licenses/>.
*/

if(msg.payload=="{\"Level\":1}"){
    msg.payload = JSON.parse('{"Level_ID":1}');
    msg.projection = JSON.parse('{"_id":0}');
    msg.limit = 3;
    msg.sort = {"Score":-1};
    msg.skip = 0;
}
if(msg.payload=="{\"Level\":2}"){
    msg.payload = JSON.parse('{"Level_ID":2}');
    msg.projection = JSON.parse('{"_id":0}');
    msg.limit = 3;
    msg.sort = {"Score":-1};
    msg.skip = 0;
}
return msg;
