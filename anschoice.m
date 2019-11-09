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
  reward=-1;  % Reward drops level of cur question by 2 levels 
  penalty=1;  % Penalty increases level of cur question by 1 level
  qlvl_mat=cell2mat(question(:,4));
  
    if strcmp(sel,num2str(question{nextq,3})) % Answer is correct
    
    if sum(qlvl_mat>curlvl)>0
    % Next higher level or max level available
    next_lvl=min(qlvl_mat(qlvl_mat>curlvl));
    else
    next_lvl=max(qlvl_mat);
    endif
    
    % Update lvls
    
    % Answered q drops a level
    question{nextq,4}=curlvl+reward;
    % Lower level questions drop 2x lower
    lwl_idx=find(qlvl_mat<curlvl);
    question(lwl_idx,4)=num2cell(cell2mat(question(lwl_idx,4))+(2*reward));
    
    
    %Some levels might be negative - moving levels to be non-negative
    if min(cell2mat(question(:,4)))<0
        corr=abs(min(cell2mat(question(:,4))));
        question(:,4)=num2cell(cell2mat(question(:,4))+abs(min(cell2mat(question(:,4)))));
        next_lvl=next_lvl+corr;
    endif

%Save 
pth=pwd;
save ([pth "/question.txt"], "question");

    
%askquestion    
          askquestion(askedq,question,next_lvl);

    
    else   % Answer is wrong
    questopts=cell2mat(question(:,4))<curlvl;
    
    if sum(questopts)>0   % Higher level question exists
      askquestion(askedq,question,curlvl-1);
    else
    askquestion(askedq,question,curlvl);   % Ask question of same level
    endif
  end
%num2str(question{3})endfunction