# MediaCycle

This project aims to slow down the never-stopping news/media cycle we have today. It allows users to configure times of the day, e.g. 6 a.m. and 5 p.m., when they would like their news, social media feeds, etc. delivered to them. New content that releases before these times will be queued until the next release window.

Queued media could be delivered via email, or perhaps through some sort of private RSS channel.

## Other Features

- Controlled aggregation levels (user specifies the degree to which they only want the top posts in their feed from the latest window)
- Non-bottomless creator/content suggestions

## ToDo

- [ ] Reorganize classes/namespaces because they are very goofy currently
- [ ] Support for config
  - [ ] Options for what properties are shown when displaying a list of folders, sources, or articles
- [ ] Proper CLI features / navigation
  - [ ] Ability to edit/remove/rename a portion of your OPML
  - [ ] Open a folder to view contents vs. open a folder and view the aggregated feed
- [ ] Tracking metrics
  - [ ] Article viewed status, settable from a directory level
  - [ ] Like/dislike
  - [ ] All stored locally
  - [ ] Can be turned off in settings/config
- [ ] Support/donation
  - [ ] Performing one-time or setting recurring reminders to donate to your favorite creaters/sources, payouts/ratios could be suggested per metrics
- [ ] Website mode vs. reader mode
- [ ] Offline support
