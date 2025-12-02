This is C# version implementation of this tutorial: https://www.youtube.com/watch?v=QPeycNt29tY&list=PLfcCiyd_V9GH8M9xd_QKlyU8jryGcy3Xa

There is a bug in E36 on the C# version tool in editor. It can't totaly clean the reference of added node when QueueFree(), so after build editor will have some error about disposed object.

Currently updated to E37.
