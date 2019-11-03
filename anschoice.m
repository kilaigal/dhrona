## Copyright (C) 2019 Arvind
## 
## This program is free software: you can redistribute it and/or modify it
## under the terms of the GNU General Public License as published by
## the Free Software Foundation, either version 3 of the License, or
## (at your option) any later version.
## 
## This program is distributed in the hope that it will be useful, but
## WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
## GNU General Public License for more details.
## 
## You should have received a copy of the GNU General Public License
## along with this program.  If not, see
## <https://www.gnu.org/licenses/>.

## -*- texinfo -*- 
## @deftypefn {} {@var{retval} =} anschoice (@var{input1}, @var{input2})
##
## @seealso{}
## @end deftypefn

## Author: Arvind <arvind@NishArvs-MacBook-Pro.local>
## Created: 2019-11-02

function anschoice(~,~,sel,askedq,question,nextq,h)
  delete(h);
      curlvl=question{nextq,4};   % Current level
  if sel==num2str(question{nextq,3}) % Answer is correct
    questopts=cell2mat(question(:,4))>curlvl;
    
    if sum(questopts)>0   % Higher level question exists
      askquestion(askedq,question,curlvl+1);
    else
    askquestion(askedq,question,curlvl);   % Ask question of same level
    endif
else
    questopts=cell2mat(question(:,4))<curlvl;
    
    if sum(questopts)>0   % Higher level question exists
      askquestion(askedq,question,curlvl-1);
    else
    askquestion(askedq,question,curlvl);   % Ask question of same level
    endif
end
%num2str(question{3})endfunction