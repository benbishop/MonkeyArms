#MonkeyArms#

MonkeyArms is a MVCS framework inspired by the AS3 framework Robotlegs. I named it MonkeyArms as a homage to the Xamarin mascot. I chose Arms instead of Legs because monkeys are known for their arms, not legs.

##What this Framework Provides ##

###DI Container Wrapper###

MonkeyArms provides a class called **DI** that is basically a wrapper for TinyIoC. With this class, developers can designate Classes as Singletons, assign Classes to Interfaces, designate Commands to **Invokers**, and map **Mediators** to anything that implements the **IMediatorTarget** Interface. Developers can retrieve any of these dependencies via the *Get* method.

###Custom Inject Attribute###

Any class that extends Actor can have a dependency injected via the **[Inject]** attribute. To provide additional flexibility, a **DIUtil** class has been provided that will allow a developer to pass a Class that implements **IInjectingTarget** to its *InjectProps* method for injection.

###Invoker/Command Map###

The framework provides an **Invoker** base class that can be designated as a Singleton and have **Command** Classes mapped to it. Anytime an **Invoker** is invoked, it will execute any **Command** registered with it. In cases where a developer may want to use an **Invoker** as a global event/signal with no command execution, an Invoked EventHandler has been provided. With this EventHandler, an **Invoker** can be mapped as a Singleton and any class can then listen for the Invoked event. 

###Mediator###

**Mediators** are the glue for this framework. As mentioned before, **Mediators** can be mapped to any classes that implement **IMediatorTarget**. A class looking to be mediated, can request a **Mediator** via the *RequestMediator* method of the **DI** class. This is especially useful for communicating changes in your data/state layer to your views. The mediators can also listen for view events to dispatch **Invokers**.

###Other Thoughts###

MonkeyArms is a micro architecture. It is meant to be more of a toolbox than a end-all-be-all solution for application architecture. You can use as much or little of it as you want. You can just use the DIUtil, or just the Invoker/Command functionality, or just the TinyIoC wrapper. It's up to you.

The goal of this framework first and foremost is to bring over mechanisms from the Flash world that we found very useful when building large scalable apps. 

###Moving Forward###

We just recently started dog-fooding Monkey Arms and it is very much in an Alpha state. There are features we'd like to add and solidify. We'd also like to get a couple of tutorial apps built. We believe in TDD and want to put the best solution out there. But good things take time and patience. If you're looking to experiment and offer input on how we can do things better, we'd love to hear from you.

However, if you are looking for the most perfect, the most documented, the most solid framework for Xamarin projects out there, we'd like to politely point you to the disclaimer at the bottom of this page. 

This is very much a work in progress.

###Example App###

We created a simple password protected contacts app that demonstrates the overall structure a MonkeyArms app follows. We also demonstrate how you can use mappings for platform specific delegates. In this case, the delegate that uses Xamarin.Mobile to access a device's contacts library.

https://github.com/benbishop/MonkeyArmsAddressBook


**Note**
If you're having issues getting the PCL to compile please check out the following link:
http://slodge.blogspot.com/2013/01/if-pcls-will-not-build-for-you-in.html

You may have to manually tweak a file in your Mono framework install to get Xamarin Studio to compile it properly.


Copyright 2013 Freckle Interactive LLC ben@freckleinteractive.net

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.