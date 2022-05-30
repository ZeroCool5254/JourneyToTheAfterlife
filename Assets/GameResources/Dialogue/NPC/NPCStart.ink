H... How can I see you? You are supposed to be dead... What is happening?
-> FirstChoices

== FirstChoices ==
* [I am dead] -> Am_Dead
+ [I wanted to see you] -> Wanted_to_see

== Am_Dead ==
But if you are dead then why can I see you? I don't understand what's happening.
-> FirstChoices

== Wanted_to_see ==
I miss you so much. It's been hard since you died. Will I see you again?
-> SecondChoices

== SecondChoices ==
* [Some day maybe] -> Some_day
* [I don't know] -> Dont_know

== Some_day ==
I suppose one day when I die too then we could see each other. Maybe its true what they say...
Death is only the beginning...
->Goodbye

== Dont_know ==
I really hope that I can see you again. Know that although you are dead, we will always love you and we will never forget you.
->Goodbye

== Goodbye ==
* [Its time for me to go] -> Time_To_Go

== Time_To_Go ==
I understand. Thank you for being here and talking to me this one last time. I.. don't know what to say other than I love you and I hope to see you again some day.
-> END