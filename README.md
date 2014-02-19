#MonkeyArms#

One of the great and maddening things about programming is that there is rarely just one way to accomplish something. This is especially true when it comes to the architecture of your application. While some programming frameworks try to coerce you to use MVC or MVVM, MonkeyArms embraces the fact that not every application should have the same arrangement.

MonkeyArms is more of a toolbox than a blueprint. MonkeyArms provides features that are independent of each other to help solve issues that programmers encounter every day while trying to write clean and portable code. These problems include decoupling, dependency injection, messaging between entities, configuration, integration with legacy code, and testability.

##Example App##

We created a simple password protected contacts app that demonstrates the overall structure a MonkeyArms app follows. We also demonstrate how you can use mappings for platform specific delegates. In this case, the delegate that uses Xamarin.Mobile to access a device's contacts library.

https://github.com/benbishop/MonkeyArmsAddressBook

##What this Framework Provides ##

###DI Container Wrapper###
http://www.youtube.com/watch?v=GP-guLA_piw

MonkeyArms provides a class called **DI** that is basically a wrapper for TinyIoC. With this class, developers can designate Classes as Singletons, assign Classes to Interfaces, designate Commands to **Invokers**, and map **Mediators** to anything that implements the **IMediatorTarget** Interface. Developers can retrieve any of these dependencies via the *Get* method.

###Custom Inject Attribute###

Any class that extends the Actor class can have a dependency injected via the **[Inject]** attribute. To provide additional flexibility, a **DIUtil** class has been provided that will allow a developer to pass a Class that implements **IInjectingTarget** to its *InjectProps* method for injection.


###Invoker/Command Map###
http://www.youtube.com/watch?v=_owgEjN2qew

The framework provides an **Invoker** base class that can be designated as a Singleton and have **Command** Classes mapped to it. Anytime an **Invoker** is invoked, it will execute any **Command** registered with it. In cases where a developer may want to use an **Invoker** as a global event/signal with no command execution, an Invoked EventHandler has been provided. With this EventHandler, an **Invoker** can be mapped as a Singleton and any class can then listen for the Invoked event. 

###Mediator###
http://www.youtube.com/watch?v=GWYd3lM4RVc

**Mediators** are the glue for this framework. As mentioned before, **Mediators** can be mapped to any classes that implement **IMediatorTarget**. A class looking to be mediated, can request a **Mediator** via the *RequestMediator* method of the **DI** class. This is especially useful for communicating changes in your data/state layer to your views. The mediators can also listen for view events to dispatch **Invokers**.

###Moving Forward/Current State###

We have used MonkeyArms in multiple projects, and we feel it is in a very strong beta state. At this juncture we'd love to receive any feedback or issues before we put out an official release.

If you want to learn more about the history and rationale for pursuing MonkeyArms check out Ben's interview on the Gone Mobile podcast:
http://gonemobile.io/blog/e0008-ben-bishops-monkey-arms/



Copyright 2014 Freckle Interactive LLC ben@freckleinteractive.net

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.