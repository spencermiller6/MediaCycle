# MediaCycle

This project aims to slow down the never-stopping news/media cycle we have today. It allows users to configure times of the day, e.g. 6 a.m. and 5 p.m., when they would like their news, social media feeds, etc. delivered to them. New content that releases before these times will be queued until the next release window.

Queued media could be delivered via email, or perhaps through some sort of private RSS channel.

## Other Features

- Controlled aggregation levels (user specifies the degree to which they only want the top posts in their feed from the latest window)
- Non-bottomless creator/content suggestions

## ToDo

Now:

- [ ] Context-aware auto-completion
- [ ] Ability to pass in a path as feed argument
- [ ] Support for relative navigation by index
- [ ] Some light consistency improvements, refactoring, warning fixes, commenting
- [ ] Refactor options
  - [ ] Could be struct? Dict?
  - [ ] Fix bug with longName
- [ ] More commands
  - [ ] Command for opening an article
    - [x] In new tab
    - [ ] As text
  - [ ] 'config' command that opens config file
  - [ ] 'subscriptions' command to open subscriptions file
- [ ] Make release time functionality more intuitive (sort by release time? only show x latest release times?)
- [x] Reorganize classes/namespaces because they are very goofy currently
- [ ] Support for config
  - [ ] Options for what properties are shown when displaying a list of folders, sources, or articles
- [ ] Implement -h and other options for commands
- [x] Proper CLI features / navigation
  - [x] Ability to edit/remove/rename a portion of your OPML
  - [x] Open a folder to view contents vs. open a folder and view the aggregated feed
     
Later:

- [ ] Tracking metrics
  - [ ] Article viewed status, settable from a directory level
  - [ ] Like/dislike
  - [ ] All stored locally
  - [ ] Can be turned off in settings/config
- [ ] Support/donation
  - [ ] Performing one-time or setting recurring reminders to donate to your favorite creaters/sources, payouts/ratios could be suggested per metrics
- [ ] Offline support
