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
## @deftypefn {} {@var{retval} =} askquestion (@var{input1}, @var{input2})
##
## @seealso{}
## @end deftypefn

## Author: Arvind <arvind@NishArvs-MacBook-Pro.local>
## Created: 2019-11-02

function askquestion(askedq,question,lvl)  % Ask questions of the given level
  if lvl< min(cell2mat(question(:,4)))   % lvl is lower than the lowest available question
  disp('Level invalid');
  return;
  else
  
        questopts=not(cell2mat(question(:,4))==lvl);  
      if sum(questopts)>0   % Questions of chosen level exist
        % Do nothing
        
      else   % Questions of selected level do not exists. Opening up all questions of lower level
        
                questopts=not(cell2mat(question(:,4))<lvl);  
      end
      invalid_idx=find(questopts);  % Index of zeros
end
  
  
  %Avoid previously asked questions
 r_tmp=randperm(length(question));      % Randomly choosing a questionif ~isempty(askedq)||~isempty(invalid_idx)r1=setdiff(r_tmp,invalid_idx);   % Eliminating asked questions or invalid questions
r=setdiff(r1,askedq);endnextq=r(1);askedq=[askedq,nextq];% create an empty dialog window titled 'Dialog Example'h = dialog ("name", "Question");q=text("parent",h,..."string",question{nextq,1},"fontsize",45);% create a button (default style)b1 = uicontrol (h, "string", num2str(question{nextq,2}{1}),... "position",[10 10 100 40],"callback",...
 {@anschoice,num2str(question{nextq,2}{1}),askedq,question,nextq,h}); b2 = uicontrol (h, "string", num2str(question{nextq,2}{2}),... "position",[135 10 100 40],"callback",...
 {@anschoice,num2str(question{nextq,2}{2}),askedq,question,nextq,h}); b3 = uicontrol (h, "string", num2str(question{nextq,2}{3}),... "position",[260 10 100 40],"callback",...
 {@anschoice,num2str(question{nextq,2}{3}),askedq,question,nextq,h}); b4 = uicontrol (h, "string", num2str(question{nextq,2}{4}),... "position",[385 10 100 40],"callback",...
 {@anschoice,num2str(question{nextq,2}{4}),askedq,question,nextq,h});endfunction