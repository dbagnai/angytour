





<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">



  <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/frameworks-d7b19415c108234b91acac0d0c02091c860993c13687a757ee345cc1ecd3a9d1.css" integrity="sha256-17GUFcEII0uRrKwNDAIJHIYJk8E2h6dX7jRcwezTqdE=" media="all" rel="stylesheet" />
  <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/github-b04ed9caff2b74257107082fbd37cff6af044e6e700c8ba91d7230b9f81a83a3.css" integrity="sha256-sE7Zyv8rdCVxBwgvvTfP9q8ETm5wDIupHXIwufgag6M=" media="all" rel="stylesheet" />
  
  
  
  

  <meta name="viewport" content="width=device-width">
  
  <title>jquery.mb.vimeo_player/jquery.mb.vimeo_player.js at master · pupunzi/jquery.mb.vimeo_player</title>
  <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="GitHub">
  <link rel="fluid-icon" href="https://github.com/fluidicon.png" title="GitHub">
  <meta property="fb:app_id" content="1401488693436528">

    
    <meta content="https://avatars0.githubusercontent.com/u/56659?v=3&amp;s=400" property="og:image" /><meta content="GitHub" property="og:site_name" /><meta content="object" property="og:type" /><meta content="pupunzi/jquery.mb.vimeo_player" property="og:title" /><meta content="https://github.com/pupunzi/jquery.mb.vimeo_player" property="og:url" /><meta content="jquery.mb.vimeo_player - A jQuery plug-in to play any Vimeo video as background of your page" property="og:description" />

  <link rel="assets" href="https://assets-cdn.github.com/">
  <link rel="web-socket" href="wss://live.github.com/_sockets/VjI6MTY1NTE2MzgwOjgyYzkwODhmM2JjYWQ0NDc5MzU0MWJkNDQ3Yzk1MWVkNDU2YzUxOGRkOTVmMGVlYTg4ZGFhMGU1NGJjZDIyOWU=--e8b931755c5306c4ba634db6e1c8b473c5823956">
  <meta name="pjax-timeout" content="1000">
  <link rel="sudo-modal" href="/sessions/sudo_modal">
  <meta name="request-id" content="764B:309D:192B63A:256BBF7:58FA0FE2" data-pjax-transient>
  

  <meta name="selected-link" value="repo_source" data-pjax-transient>

  <meta name="google-site-verification" content="KT5gs8h0wvaagLKAVWq8bbeNwnZZK1r1XQysX3xurLU">
<meta name="google-site-verification" content="ZzhVyEFwb7w3e0-uOTltm8Jsck2F5StVihD0exw2fsA">
    <meta name="google-analytics" content="UA-3769691-2">

<meta content="collector.githubapp.com" name="octolytics-host" /><meta content="github" name="octolytics-app-id" /><meta content="https://collector.githubapp.com/github-external/browser_event" name="octolytics-event-url" /><meta content="764B:309D:192B63A:256BBF7:58FA0FE2" name="octolytics-dimension-request_id" /><meta content="19534130" name="octolytics-actor-id" /><meta content="dbagnai" name="octolytics-actor-login" /><meta content="659e05745818fd49e639c183fcfe7f1d494ee3deb4b56ef38d74fd4b01b33d86" name="octolytics-actor-hash" />
<meta content="/&lt;user-name&gt;/&lt;repo-name&gt;/blob/show" data-pjax-transient="true" name="analytics-location" />




  <meta class="js-ga-set" name="dimension1" content="Logged In">


  

      <meta name="hostname" content="github.com">
  <meta name="user-login" content="dbagnai">

      <meta name="expected-hostname" content="github.com">
    <meta name="js-proxy-site-detection-payload" content="NjY1MzBkMzc3Zjk4ZjNmMzhjMGY4NWFmOTZkMjMwMTg0ZWY4MDU0MTAxNzUzMjcwYzllMThmMGM2NjAwZDNlNnx7InJlbW90ZV9hZGRyZXNzIjoiNDYuMTAyLjExMi4xMTciLCJyZXF1ZXN0X2lkIjoiNzY0QjozMDlEOjE5MkI2M0E6MjU2QkJGNzo1OEZBMEZFMiIsInRpbWVzdGFtcCI6MTQ5Mjc4MzA4MiwiaG9zdCI6ImdpdGh1Yi5jb20ifQ==">


  <meta name="html-safe-nonce" content="bb49d64ddaddff1268e84314835b8c50888ff1e9">

  <meta http-equiv="x-pjax-version" content="f4648466cb78a5dbe0fcbb996e5a9fed">
  

    
  <meta name="description" content="jquery.mb.vimeo_player - A jQuery plug-in to play any Vimeo video as background of your page">
  <meta name="go-import" content="github.com/pupunzi/jquery.mb.vimeo_player git https://github.com/pupunzi/jquery.mb.vimeo_player.git">

  <meta content="56659" name="octolytics-dimension-user_id" /><meta content="pupunzi" name="octolytics-dimension-user_login" /><meta content="78282440" name="octolytics-dimension-repository_id" /><meta content="pupunzi/jquery.mb.vimeo_player" name="octolytics-dimension-repository_nwo" /><meta content="true" name="octolytics-dimension-repository_public" /><meta content="false" name="octolytics-dimension-repository_is_fork" /><meta content="78282440" name="octolytics-dimension-repository_network_root_id" /><meta content="pupunzi/jquery.mb.vimeo_player" name="octolytics-dimension-repository_network_root_nwo" />
        <link href="https://github.com/pupunzi/jquery.mb.vimeo_player/commits/master.atom" rel="alternate" title="Recent Commits to jquery.mb.vimeo_player:master" type="application/atom+xml">


    <link rel="canonical" href="https://github.com/pupunzi/jquery.mb.vimeo_player/blob/master/dist/jquery.mb.vimeo_player.js" data-pjax-transient>


  <meta name="browser-stats-url" content="https://api.github.com/_private/browser/stats">

  <meta name="browser-errors-url" content="https://api.github.com/_private/browser/errors">

  <link rel="mask-icon" href="https://assets-cdn.github.com/pinned-octocat.svg" color="#000000">
  <link rel="icon" type="image/x-icon" href="https://assets-cdn.github.com/favicon.ico">

<meta name="theme-color" content="#1e2327">


  <meta name="u2f-support" content="true">

  </head>

  <body class="logged-in env-production page-blob">
    


  <div class="position-relative js-header-wrapper ">
    <a href="#start-of-content" tabindex="1" class="accessibility-aid js-skip-to-content">Skip to content</a>
    <div id="js-pjax-loader-bar" class="pjax-loader-bar"><div class="progress"></div></div>

    
    
    



        
<div class="header" role="banner">
  <div class="container clearfix">
    <a class="header-logo-invertocat" href="https://github.com/" data-hotkey="g d" aria-label="Homepage" data-ga-click="Header, go to dashboard, icon:logo">
  <svg aria-hidden="true" class="octicon octicon-mark-github" height="32" version="1.1" viewBox="0 0 16 16" width="32"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0 0 16 8c0-4.42-3.58-8-8-8z"/></svg>
</a>


        <div class="header-search scoped-search site-scoped-search js-site-search" role="search">
  <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/pupunzi/jquery.mb.vimeo_player/search" class="js-site-search-form" data-scoped-search-url="/pupunzi/jquery.mb.vimeo_player/search" data-unscoped-search-url="/search" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
    <label class="form-control header-search-wrapper js-chromeless-input-container">
        <a href="/pupunzi/jquery.mb.vimeo_player/blob/master/dist/jquery.mb.vimeo_player.js" class="header-search-scope no-underline">This repository</a>
      <input type="text"
        class="form-control header-search-input js-site-search-focus js-site-search-field is-clearable"
        data-hotkey="s"
        name="q"
        value=""
        placeholder="Search"
        aria-label="Search this repository"
        data-unscoped-placeholder="Search GitHub"
        data-scoped-placeholder="Search"
        autocapitalize="off">
        <input type="hidden" class="js-site-search-type-field" name="type" >
    </label>
</form></div>


      <ul class="header-nav float-left" role="navigation">
        <li class="header-nav-item">
          <a href="/pulls" aria-label="Pull requests you created" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:pulls context:user" data-hotkey="g p" data-selected-links="/pulls /pulls/assigned /pulls/mentioned /pulls">
            Pull requests
</a>        </li>
        <li class="header-nav-item">
          <a href="/issues" aria-label="Issues you created" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:issues context:user" data-hotkey="g i" data-selected-links="/issues /issues/assigned /issues/mentioned /issues">
            Issues
</a>        </li>
          <li class="header-nav-item">
            <a class="header-nav-link" href="https://gist.github.com/" data-ga-click="Header, go to gist, text:gist">Gist</a>
          </li>
      </ul>

    
<ul class="header-nav user-nav float-right" id="user-links">
  <li class="header-nav-item">
    

  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link tooltipped tooltipped-s js-menu-target" href="/new"
       aria-label="Create new…"
       data-ga-click="Header, create new, icon:add">
      <svg aria-hidden="true" class="octicon octicon-plus float-left" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 9H7v5H5V9H0V7h5V2h2v5h5z"/></svg>
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <ul class="dropdown-menu dropdown-menu-sw">
        
<a class="dropdown-item" href="/new" data-ga-click="Header, create new repository">
  New repository
</a>

  <a class="dropdown-item" href="/new/import" data-ga-click="Header, import a repository">
    Import repository
  </a>

<a class="dropdown-item" href="https://gist.github.com/" data-ga-click="Header, create new gist">
  New gist
</a>

  <a class="dropdown-item" href="/organizations/new" data-ga-click="Header, create new organization">
    New organization
  </a>



  <div class="dropdown-divider"></div>
  <div class="dropdown-header">
    <span title="pupunzi/jquery.mb.vimeo_player">This repository</span>
  </div>
    <a class="dropdown-item" href="/pupunzi/jquery.mb.vimeo_player/issues/new" data-ga-click="Header, create new issue">
      New issue
    </a>

      </ul>
    </div>
  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link name tooltipped tooltipped-sw js-menu-target" href="/dbagnai"
       aria-label="View profile and more"
       data-ga-click="Header, show menu, icon:avatar">
      <img alt="@dbagnai" class="avatar" src="https://avatars3.githubusercontent.com/u/19534130?v=3&amp;s=40" height="20" width="20">
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <div class="dropdown-menu dropdown-menu-sw">
        <div class="dropdown-header header-nav-current-user css-truncate">
          Signed in as <strong class="css-truncate-target">dbagnai</strong>
        </div>

        <div class="dropdown-divider"></div>

        <a class="dropdown-item" href="/dbagnai" data-ga-click="Header, go to profile, text:your profile">
          Your profile
        </a>
        <a class="dropdown-item" href="/dbagnai?tab=stars" data-ga-click="Header, go to starred repos, text:your stars">
          Your stars
        </a>
        <a class="dropdown-item" href="/explore" data-ga-click="Header, go to explore, text:explore">
          Explore
        </a>
          <a class="dropdown-item" href="/integrations" data-ga-click="Header, go to integrations, text:integrations">
            Integrations
          </a>
        <a class="dropdown-item" href="https://help.github.com" data-ga-click="Header, go to help, text:help">
          Help
        </a>

        <div class="dropdown-divider"></div>

        <a class="dropdown-item" href="/settings/profile" data-ga-click="Header, go to settings, icon:settings">
          Settings
        </a>

        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/logout" class="logout-form" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="ftSE5Yp4qfvOEdMOlFa8bpJ6736atJYqWas7PoytGb/QX8C2dpWqCPxrKlnnRNdMZoUH1RDkJs3eAclWhnIL/Q==" /></div>
          <button type="submit" class="dropdown-item dropdown-signout" data-ga-click="Header, sign out, icon:logout">
            Sign out
          </button>
</form>      </div>
    </div>
  </li>
</ul>


    <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/logout" class="sr-only right-0" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="r5tMKpLS0tNcnOYk1++r9dAszMO4mcdA78mRrp0/xdUBEAh5bj/RIG7mH3Ok/cDXJNMkaDLJd6doY2PGl+DXlw==" /></div>
      <button type="submit" class="dropdown-item dropdown-signout" data-ga-click="Header, sign out, icon:logout">
        Sign out
      </button>
</form>  </div>
</div>


      

  </div>

  <div id="start-of-content" class="accessibility-aid"></div>

    <div id="js-flash-container">
</div>



  <div role="main">
        <div itemscope itemtype="http://schema.org/SoftwareSourceCode">
    <div id="js-repo-pjax-container" data-pjax-container>
        


  <div class="pagehead repohead instapaper_ignore readability-menu experiment-repo-nav">
    <div class="container repohead-details-container">


      <ul class="pagehead-actions">
  <li>
        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/notifications/subscribe" class="js-social-container" data-autosubmit="true" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="DjcbRxfOoK47crNCm8Je4D2uWImOTaAm8f7BiuhgSMWWhz2+4eqcmRbWOjLQ+RMARQSlWwXSZoYJ/Mvz96m7jw==" /></div>      <input class="form-control" id="repository_id" name="repository_id" type="hidden" value="78282440" />

        <div class="select-menu js-menu-container js-select-menu">
          <a href="/pupunzi/jquery.mb.vimeo_player/subscription"
            class="btn btn-sm btn-with-count select-menu-button js-menu-target" role="button" tabindex="0" aria-haspopup="true"
            data-ga-click="Repository, click Watch settings, action:blob#show">
            <span class="js-select-button">
              <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
              Watch
            </span>
          </a>
          <a class="social-count js-social-count"
            href="/pupunzi/jquery.mb.vimeo_player/watchers"
            aria-label="2 users are watching this repository">
            2
          </a>

        <div class="select-menu-modal-holder">
          <div class="select-menu-modal subscription-menu-modal js-menu-content">
            <div class="select-menu-header js-navigation-enable" tabindex="-1">
              <svg aria-label="Close" class="octicon octicon-x js-menu-close" height="16" role="img" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
              <span class="select-menu-title">Notifications</span>
            </div>

              <div class="select-menu-list js-navigation-container" role="menu">

                <div class="select-menu-item js-navigation-item selected" role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input checked="checked" id="do_included" name="do" type="radio" value="included" />
                    <span class="select-menu-item-heading">Not watching</span>
                    <span class="description">Be notified when participating or @mentioned.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
                      Watch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input id="do_subscribed" name="do" type="radio" value="subscribed" />
                    <span class="select-menu-item-heading">Watching</span>
                    <span class="description">Be notified of all conversations.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
                      Unwatch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input id="do_ignore" name="do" type="radio" value="ignore" />
                    <span class="select-menu-item-heading">Ignoring</span>
                    <span class="description">Never be notified.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-mute" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8 2.81v10.38c0 .67-.81 1-1.28.53L3 10H1c-.55 0-1-.45-1-1V7c0-.55.45-1 1-1h2l3.72-3.72C7.19 1.81 8 2.14 8 2.81zm7.53 3.22l-1.06-1.06-1.97 1.97-1.97-1.97-1.06 1.06L11.44 8 9.47 9.97l1.06 1.06 1.97-1.97 1.97 1.97 1.06-1.06L13.56 8l1.97-1.97z"/></svg>
                      Stop ignoring
                    </span>
                  </div>
                </div>

              </div>

            </div>
          </div>
        </div>
</form>
  </li>

  <li>
      <div class="js-toggler-container js-social-container starring-container ">
    <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/pupunzi/jquery.mb.vimeo_player/unstar" class="starred" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="m6dt0/dmr0XCDdGdLDg40Gsa5MozyyskDkZERV8kRtS2JLkhHWMNk4quWsEhAt9lx1VNHzBtwfe+/9BrnYLi2A==" /></div>
      <button
        type="submit"
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Unstar this repository" title="Unstar pupunzi/jquery.mb.vimeo_player"
        data-ga-click="Repository, click unstar button, action:blob#show; text:Unstar">
        <svg aria-hidden="true" class="octicon octicon-star" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M14 6l-4.9-.64L7 1 4.9 5.36 0 6l3.6 3.26L2.67 14 7 11.67 11.33 14l-.93-4.74z"/></svg>
        Unstar
      </button>
        <a class="social-count js-social-count" href="/pupunzi/jquery.mb.vimeo_player/stargazers"
           aria-label="6 users starred this repository">
          6
        </a>
</form>
    <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/pupunzi/jquery.mb.vimeo_player/star" class="unstarred" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="PGGr+Mw8cIvfoYMastppQJGL+m6lZperxTChgXKL/Ax6GHPcbtSwTrZp2oWVVL3n+TB4F9mXXBthVVXGXD7q2g==" /></div>
      <button
        type="submit"
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Star this repository" title="Star pupunzi/jquery.mb.vimeo_player"
        data-ga-click="Repository, click star button, action:blob#show; text:Star">
        <svg aria-hidden="true" class="octicon octicon-star" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M14 6l-4.9-.64L7 1 4.9 5.36 0 6l3.6 3.26L2.67 14 7 11.67 11.33 14l-.93-4.74z"/></svg>
        Star
      </button>
        <a class="social-count js-social-count" href="/pupunzi/jquery.mb.vimeo_player/stargazers"
           aria-label="6 users starred this repository">
          6
        </a>
</form>  </div>

  </li>

  <li>
          <a href="#fork-destination-box" class="btn btn-sm btn-with-count"
              title="Fork your own copy of pupunzi/jquery.mb.vimeo_player to your account"
              aria-label="Fork your own copy of pupunzi/jquery.mb.vimeo_player to your account"
              rel="facebox"
              data-ga-click="Repository, show fork modal, action:blob#show; text:Fork">
              <svg aria-hidden="true" class="octicon octicon-repo-forked" height="16" version="1.1" viewBox="0 0 10 16" width="10"><path fill-rule="evenodd" d="M8 1a1.993 1.993 0 0 0-1 3.72V6L5 8 3 6V4.72A1.993 1.993 0 0 0 2 1a1.993 1.993 0 0 0-1 3.72V6.5l3 3v1.78A1.993 1.993 0 0 0 5 15a1.993 1.993 0 0 0 1-3.72V9.5l3-3V4.72A1.993 1.993 0 0 0 8 1zM2 4.2C1.34 4.2.8 3.65.8 3c0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zm3 10c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zm3-10c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2z"/></svg>
            Fork
          </a>

          <div id="fork-destination-box" style="display: none;">
            <h2 class="facebox-header" data-facebox-id="facebox-header">Where should we fork this repository?</h2>
            <include-fragment src=""
                class="js-fork-select-fragment fork-select-fragment"
                data-url="/pupunzi/jquery.mb.vimeo_player/fork?fragment=1">
              <img alt="Loading" height="64" src="https://assets-cdn.github.com/images/spinners/octocat-spinner-128.gif" width="64" />
            </include-fragment>
          </div>

    <a href="/pupunzi/jquery.mb.vimeo_player/network" class="social-count"
       aria-label="1 user forked this repository">
      1
    </a>
  </li>
</ul>

      <h1 class="public ">
  <svg aria-hidden="true" class="octicon octicon-repo" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M4 9H3V8h1v1zm0-3H3v1h1V6zm0-2H3v1h1V4zm0-2H3v1h1V2zm8-1v12c0 .55-.45 1-1 1H6v2l-1.5-1.5L3 16v-2H1c-.55 0-1-.45-1-1V1c0-.55.45-1 1-1h10c.55 0 1 .45 1 1zm-1 10H1v2h2v-1h3v1h5v-2zm0-10H2v9h9V1z"/></svg>
  <span class="author" itemprop="author"><a href="/pupunzi" class="url fn" rel="author">pupunzi</a></span><!--
--><span class="path-divider">/</span><!--
--><strong itemprop="name"><a href="/pupunzi/jquery.mb.vimeo_player" data-pjax="#js-repo-pjax-container">jquery.mb.vimeo_player</a></strong>

</h1>

    </div>
    <div class="container">
      
<nav class="reponav js-repo-nav js-sidenav-container-pjax"
     itemscope
     itemtype="http://schema.org/BreadcrumbList"
     role="navigation"
     data-pjax="#js-repo-pjax-container">

  <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
    <a href="/pupunzi/jquery.mb.vimeo_player" class="js-selected-navigation-item selected reponav-item" data-hotkey="g c" data-selected-links="repo_source repo_downloads repo_commits repo_releases repo_tags repo_branches /pupunzi/jquery.mb.vimeo_player" itemprop="url">
      <svg aria-hidden="true" class="octicon octicon-code" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M9.5 3L8 4.5 11.5 8 8 11.5 9.5 13 14 8 9.5 3zm-5 0L0 8l4.5 5L6 11.5 2.5 8 6 4.5 4.5 3z"/></svg>
      <span itemprop="name">Code</span>
      <meta itemprop="position" content="1">
</a>  </span>

    <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
      <a href="/pupunzi/jquery.mb.vimeo_player/issues" class="js-selected-navigation-item reponav-item" data-hotkey="g i" data-selected-links="repo_issues repo_labels repo_milestones /pupunzi/jquery.mb.vimeo_player/issues" itemprop="url">
        <svg aria-hidden="true" class="octicon octicon-issue-opened" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M7 2.3c3.14 0 5.7 2.56 5.7 5.7s-2.56 5.7-5.7 5.7A5.71 5.71 0 0 1 1.3 8c0-3.14 2.56-5.7 5.7-5.7zM7 1C3.14 1 0 4.14 0 8s3.14 7 7 7 7-3.14 7-7-3.14-7-7-7zm1 3H6v5h2V4zm0 6H6v2h2v-2z"/></svg>
        <span itemprop="name">Issues</span>
        <span class="Counter">0</span>
        <meta itemprop="position" content="2">
</a>    </span>

  <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
    <a href="/pupunzi/jquery.mb.vimeo_player/pulls" class="js-selected-navigation-item reponav-item" data-hotkey="g p" data-selected-links="repo_pulls /pupunzi/jquery.mb.vimeo_player/pulls" itemprop="url">
      <svg aria-hidden="true" class="octicon octicon-git-pull-request" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M11 11.28V5c-.03-.78-.34-1.47-.94-2.06C9.46 2.35 8.78 2.03 8 2H7V0L4 3l3 3V4h1c.27.02.48.11.69.31.21.2.3.42.31.69v6.28A1.993 1.993 0 0 0 10 15a1.993 1.993 0 0 0 1-3.72zm-1 2.92c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zM4 3c0-1.11-.89-2-2-2a1.993 1.993 0 0 0-1 3.72v6.56A1.993 1.993 0 0 0 2 15a1.993 1.993 0 0 0 1-3.72V4.72c.59-.34 1-.98 1-1.72zm-.8 10c0 .66-.55 1.2-1.2 1.2-.65 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2zM2 4.2C1.34 4.2.8 3.65.8 3c0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2z"/></svg>
      <span itemprop="name">Pull requests</span>
      <span class="Counter">0</span>
      <meta itemprop="position" content="3">
</a>  </span>

    <a href="/pupunzi/jquery.mb.vimeo_player/projects" class="js-selected-navigation-item reponav-item" data-selected-links="repo_projects new_repo_project repo_project /pupunzi/jquery.mb.vimeo_player/projects">
      <svg aria-hidden="true" class="octicon octicon-project" height="16" version="1.1" viewBox="0 0 15 16" width="15"><path fill-rule="evenodd" d="M10 12h3V2h-3v10zm-4-2h3V2H6v8zm-4 4h3V2H2v12zm-1 1h13V1H1v14zM14 0H1a1 1 0 0 0-1 1v14a1 1 0 0 0 1 1h13a1 1 0 0 0 1-1V1a1 1 0 0 0-1-1z"/></svg>
      Projects
      <span class="Counter" >0</span>
</a>
    <a href="/pupunzi/jquery.mb.vimeo_player/wiki" class="js-selected-navigation-item reponav-item" data-hotkey="g w" data-selected-links="repo_wiki /pupunzi/jquery.mb.vimeo_player/wiki">
      <svg aria-hidden="true" class="octicon octicon-book" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M3 5h4v1H3V5zm0 3h4V7H3v1zm0 2h4V9H3v1zm11-5h-4v1h4V5zm0 2h-4v1h4V7zm0 2h-4v1h4V9zm2-6v9c0 .55-.45 1-1 1H9.5l-1 1-1-1H2c-.55 0-1-.45-1-1V3c0-.55.45-1 1-1h5.5l1 1 1-1H15c.55 0 1 .45 1 1zm-8 .5L7.5 3H2v9h6V3.5zm7-.5H9.5l-.5.5V12h6V3z"/></svg>
      Wiki
</a>

  <a href="/pupunzi/jquery.mb.vimeo_player/pulse" class="js-selected-navigation-item reponav-item" data-selected-links="pulse /pupunzi/jquery.mb.vimeo_player/pulse">
    <svg aria-hidden="true" class="octicon octicon-pulse" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M11.5 8L8.8 5.4 6.6 8.5 5.5 1.6 2.38 8H0v2h3.6l.9-1.8.9 5.4L9 8.5l1.6 1.5H14V8z"/></svg>
    Pulse
</a>
  <a href="/pupunzi/jquery.mb.vimeo_player/graphs" class="js-selected-navigation-item reponav-item" data-selected-links="repo_graphs repo_contributors /pupunzi/jquery.mb.vimeo_player/graphs">
    <svg aria-hidden="true" class="octicon octicon-graph" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M16 14v1H0V0h1v14h15zM5 13H3V8h2v5zm4 0H7V3h2v10zm4 0h-2V6h2v7z"/></svg>
    Graphs
</a>

</nav>

    </div>
  </div>

<div class="container new-discussion-timeline experiment-repo-nav">
  <div class="repository-content">

    
          

<a href="/pupunzi/jquery.mb.vimeo_player/blob/c31d500b6ac45752b82686229d6a419556b79e87/dist/jquery.mb.vimeo_player.js" class="d-none js-permalink-shortcut" data-hotkey="y">Permalink</a>

<!-- blob contrib key: blob_contributors:v21:d9fc7a04c486c7aa26d0b90da91c8e76 -->

<div class="file-navigation js-zeroclipboard-container">
  
<div class="select-menu branch-select-menu js-menu-container js-select-menu float-left">
  <button class=" btn btn-sm select-menu-button js-menu-target css-truncate" data-hotkey="w"
    
    type="button" aria-label="Switch branches or tags" tabindex="0" aria-haspopup="true">
      <i>Branch:</i>
      <span class="js-select-button css-truncate-target">master</span>
  </button>

  <div class="select-menu-modal-holder js-menu-content js-navigation-container" data-pjax>

    <div class="select-menu-modal">
      <div class="select-menu-header">
        <svg aria-label="Close" class="octicon octicon-x js-menu-close" height="16" role="img" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
        <span class="select-menu-title">Switch branches/tags</span>
      </div>

      <div class="select-menu-filters">
        <div class="select-menu-text-filter">
          <input type="text" aria-label="Filter branches/tags" id="context-commitish-filter-field" class="form-control js-filterable-field js-navigation-enable" placeholder="Filter branches/tags">
        </div>
        <div class="select-menu-tabs">
          <ul>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="branches" data-filter-placeholder="Filter branches/tags" class="js-select-menu-tab" role="tab">Branches</a>
            </li>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="tags" data-filter-placeholder="Find a tag…" class="js-select-menu-tab" role="tab">Tags</a>
            </li>
          </ul>
        </div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="branches" role="menu">

        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open selected"
               href="/pupunzi/jquery.mb.vimeo_player/blob/master/dist/jquery.mb.vimeo_player.js"
               data-name="master"
               data-skip-pjax="true"
               rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target js-select-menu-filter-text">
                master
              </span>
            </a>
        </div>

          <div class="select-menu-no-results">Nothing to show</div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="tags">
        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.8/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.8"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.8">
                1.0.8
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.7/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.7"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.7">
                1.0.7
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.6/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.6"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.6">
                1.0.6
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.5/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.5"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.5">
                1.0.5
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.4/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.4"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.4">
                1.0.4
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.3/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.3"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.3">
                1.0.3
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.2/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.2"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.2">
                1.0.2
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/pupunzi/jquery.mb.vimeo_player/tree/1.0.1/dist/jquery.mb.vimeo_player.js"
              data-name="1.0.1"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="1.0.1">
                1.0.1
              </span>
            </a>
        </div>

        <div class="select-menu-no-results">Nothing to show</div>
      </div>

    </div>
  </div>
</div>

  <div class="BtnGroup float-right">
    <a href="/pupunzi/jquery.mb.vimeo_player/find/master"
          class="js-pjax-capture-input btn btn-sm BtnGroup-item"
          data-pjax
          data-hotkey="t">
      Find file
    </a>
    <button aria-label="Copy file path to clipboard" class="js-zeroclipboard btn btn-sm BtnGroup-item tooltipped tooltipped-s" data-copied-hint="Copied!" type="button">Copy path</button>
  </div>
  <div class="breadcrumb js-zeroclipboard-target">
    <span class="repo-root js-repo-root"><span class="js-path-segment"><a href="/pupunzi/jquery.mb.vimeo_player"><span>jquery.mb.vimeo_player</span></a></span></span><span class="separator">/</span><span class="js-path-segment"><a href="/pupunzi/jquery.mb.vimeo_player/tree/master/dist"><span>dist</span></a></span><span class="separator">/</span><strong class="final-path">jquery.mb.vimeo_player.js</strong>
  </div>
</div>


<include-fragment class="commit-tease" src="/pupunzi/jquery.mb.vimeo_player/contributors/master/dist/jquery.mb.vimeo_player.js">
  <div>
    Fetching contributors&hellip;
  </div>

  <div class="commit-tease-contributors">
    <img alt="" class="loader-loading float-left" height="16" src="https://assets-cdn.github.com/images/spinners/octocat-spinner-32-EAF2F5.gif" width="16" />
    <span class="loader-error">Cannot retrieve contributors at this time</span>
  </div>
</include-fragment>
<div class="file">
  <div class="file-header">
  <div class="file-actions">

    <div class="BtnGroup">
      <a href="/pupunzi/jquery.mb.vimeo_player/raw/master/dist/jquery.mb.vimeo_player.js" class="btn btn-sm BtnGroup-item" id="raw-url">Raw</a>
        <a href="/pupunzi/jquery.mb.vimeo_player/blame/master/dist/jquery.mb.vimeo_player.js" class="btn btn-sm js-update-url-with-hash BtnGroup-item" data-hotkey="b">Blame</a>
      <a href="/pupunzi/jquery.mb.vimeo_player/commits/master/dist/jquery.mb.vimeo_player.js" class="btn btn-sm BtnGroup-item" rel="nofollow">History</a>
    </div>

        <a class="btn-octicon tooltipped tooltipped-nw"
           href="https://windows.github.com"
           aria-label="Open this file in GitHub Desktop"
           data-ga-click="Repository, open with desktop, type:windows">
            <svg aria-hidden="true" class="octicon octicon-device-desktop" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M15 2H1c-.55 0-1 .45-1 1v9c0 .55.45 1 1 1h5.34c-.25.61-.86 1.39-2.34 2h8c-1.48-.61-2.09-1.39-2.34-2H15c.55 0 1-.45 1-1V3c0-.55-.45-1-1-1zm0 9H1V3h14v8z"/></svg>
        </a>

        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/pupunzi/jquery.mb.vimeo_player/edit/master/dist/jquery.mb.vimeo_player.js" class="inline-form js-update-url-with-hash" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="U0jIm0yreUyQFA2iv1XKPdAxI+3EDXi1Fe5w5fK/HdkEl7sWnWCjd7XzCJss9AiUSTlgy8OVf+xdJlyhCJKEVA==" /></div>
          <button class="btn-octicon tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and edit the file" data-hotkey="e" data-disable-with>
            <svg aria-hidden="true" class="octicon octicon-pencil" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M0 12v3h3l8-8-3-3-8 8zm3 2H1v-2h1v1h1v1zm10.3-9.3L12 6 9 3l1.3-1.3a.996.996 0 0 1 1.41 0l1.59 1.59c.39.39.39 1.02 0 1.41z"/></svg>
          </button>
</form>        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/pupunzi/jquery.mb.vimeo_player/delete/master/dist/jquery.mb.vimeo_player.js" class="inline-form" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="9RcuVcDmWISQMI8COYhBtpeduV3KHm/bAFlO7L7g9d7kB6GKYKNkHYsCUcQysI8p0n5zanc7x1a179caKUxMWA==" /></div>
          <button class="btn-octicon btn-octicon-danger tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and delete the file" data-disable-with>
            <svg aria-hidden="true" class="octicon octicon-trashcan" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M11 2H9c0-.55-.45-1-1-1H5c-.55 0-1 .45-1 1H2c-.55 0-1 .45-1 1v1c0 .55.45 1 1 1v9c0 .55.45 1 1 1h7c.55 0 1-.45 1-1V5c.55 0 1-.45 1-1V3c0-.55-.45-1-1-1zm-1 12H3V5h1v8h1V5h1v8h1V5h1v8h1V5h1v9zm1-10H2V3h9v1z"/></svg>
          </button>
</form>  </div>

  <div class="file-info">
      1052 lines (858 sloc)
      <span class="file-info-divider"></span>
    47.9 KB
  </div>
</div>

  

  <div itemprop="text" class="blob-wrapper data type-javascript">
      <table class="highlight tab-size js-file-line-container" data-tab-size="8">
      <tr>
        <td id="L1" class="blob-num js-line-number" data-line-number="1"></td>
        <td id="LC1" class="blob-code blob-code-inner js-file-line"><span class="pl-c">/*___________________________________________________________________________________________________________________________________________________</span></td>
      </tr>
      <tr>
        <td id="L2" class="blob-num js-line-number" data-line-number="2"></td>
        <td id="LC2" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ jquery.mb.components                                                                                                                             _</span></td>
      </tr>
      <tr>
        <td id="L3" class="blob-num js-line-number" data-line-number="3"></td>
        <td id="LC3" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L4" class="blob-num js-line-number" data-line-number="4"></td>
        <td id="LC4" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ file: jquery.mb.vimeo_player.js                                                                                                                   _</span></td>
      </tr>
      <tr>
        <td id="L5" class="blob-num js-line-number" data-line-number="5"></td>
        <td id="LC5" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ last modified: 26/12/16 15.39                                                                                                                    _</span></td>
      </tr>
      <tr>
        <td id="L6" class="blob-num js-line-number" data-line-number="6"></td>
        <td id="LC6" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L7" class="blob-num js-line-number" data-line-number="7"></td>
        <td id="LC7" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Open Lab s.r.l., Florence - Italy                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L8" class="blob-num js-line-number" data-line-number="8"></td>
        <td id="LC8" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L9" class="blob-num js-line-number" data-line-number="9"></td>
        <td id="LC9" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ email: matteo@open-lab.com                                                                                                                       _</span></td>
      </tr>
      <tr>
        <td id="L10" class="blob-num js-line-number" data-line-number="10"></td>
        <td id="LC10" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ site: http://pupunzi.com                                                                                                                         _</span></td>
      </tr>
      <tr>
        <td id="L11" class="blob-num js-line-number" data-line-number="11"></td>
        <td id="LC11" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _       http://open-lab.com                                                                                                                        _</span></td>
      </tr>
      <tr>
        <td id="L12" class="blob-num js-line-number" data-line-number="12"></td>
        <td id="LC12" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ blog: http://pupunzi.open-lab.com                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L13" class="blob-num js-line-number" data-line-number="13"></td>
        <td id="LC13" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Q&amp;A:  http://jquery.pupunzi.com                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L14" class="blob-num js-line-number" data-line-number="14"></td>
        <td id="LC14" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L15" class="blob-num js-line-number" data-line-number="15"></td>
        <td id="LC15" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Licences: MIT, GPL                                                                                                                               _</span></td>
      </tr>
      <tr>
        <td id="L16" class="blob-num js-line-number" data-line-number="16"></td>
        <td id="LC16" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.opensource.org/licenses/mit-license.php                                                                                            _</span></td>
      </tr>
      <tr>
        <td id="L17" class="blob-num js-line-number" data-line-number="17"></td>
        <td id="LC17" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.gnu.org/licenses/gpl.html                                                                                                          _</span></td>
      </tr>
      <tr>
        <td id="L18" class="blob-num js-line-number" data-line-number="18"></td>
        <td id="LC18" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L19" class="blob-num js-line-number" data-line-number="19"></td>
        <td id="LC19" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Copyright (c) 2001-2016. Matteo Bicocchi (Pupunzi);                                                                                              _</span></td>
      </tr>
      <tr>
        <td id="L20" class="blob-num js-line-number" data-line-number="20"></td>
        <td id="LC20" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> ___________________________________________________________________________________________________________________________________________________*/</span></td>
      </tr>
      <tr>
        <td id="L21" class="blob-num js-line-number" data-line-number="21"></td>
        <td id="LC21" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L22" class="blob-num js-line-number" data-line-number="22"></td>
        <td id="LC22" class="blob-code blob-code-inner js-file-line"><span class="pl-k">var</span> <span class="pl-en">get_vimeo_videoID</span> <span class="pl-k">=</span> <span class="pl-k">function</span>( <span class="pl-smi">url</span> ) {</td>
      </tr>
      <tr>
        <td id="L23" class="blob-num js-line-number" data-line-number="23"></td>
        <td id="LC23" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L24" class="blob-num js-line-number" data-line-number="24"></td>
        <td id="LC24" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">var</span> videoID;</td>
      </tr>
      <tr>
        <td id="L25" class="blob-num js-line-number" data-line-number="25"></td>
        <td id="LC25" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">if</span>( <span class="pl-smi">url</span>.<span class="pl-c1">indexOf</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo.com<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">&gt;</span> <span class="pl-c1">0</span> ) {</td>
      </tr>
      <tr>
        <td id="L26" class="blob-num js-line-number" data-line-number="26"></td>
        <td id="LC26" class="blob-code blob-code-inner js-file-line">		videoID <span class="pl-k">=</span> <span class="pl-smi">url</span>.<span class="pl-c1">substr</span>( <span class="pl-smi">url</span>.<span class="pl-c1">lastIndexOf</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>/<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">+</span> <span class="pl-c1">1</span>, <span class="pl-smi">url</span>.<span class="pl-c1">length</span> );</td>
      </tr>
      <tr>
        <td id="L27" class="blob-num js-line-number" data-line-number="27"></td>
        <td id="LC27" class="blob-code blob-code-inner js-file-line">	} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L28" class="blob-num js-line-number" data-line-number="28"></td>
        <td id="LC28" class="blob-code blob-code-inner js-file-line">		videoID <span class="pl-k">=</span> <span class="pl-smi">url</span>.<span class="pl-c1">length</span> <span class="pl-k">&gt;</span> <span class="pl-c1">15</span> <span class="pl-k">?</span> <span class="pl-c1">null</span> <span class="pl-k">:</span> url;</td>
      </tr>
      <tr>
        <td id="L29" class="blob-num js-line-number" data-line-number="29"></td>
        <td id="LC29" class="blob-code blob-code-inner js-file-line">	}</td>
      </tr>
      <tr>
        <td id="L30" class="blob-num js-line-number" data-line-number="30"></td>
        <td id="LC30" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L31" class="blob-num js-line-number" data-line-number="31"></td>
        <td id="LC31" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">return</span> videoID</td>
      </tr>
      <tr>
        <td id="L32" class="blob-num js-line-number" data-line-number="32"></td>
        <td id="LC32" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L33" class="blob-num js-line-number" data-line-number="33"></td>
        <td id="LC33" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L34" class="blob-num js-line-number" data-line-number="34"></td>
        <td id="LC34" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L35" class="blob-num js-line-number" data-line-number="35"></td>
        <td id="LC35" class="blob-code blob-code-inner js-file-line">( <span class="pl-k">function</span>( <span class="pl-smi">$</span> ) {</td>
      </tr>
      <tr>
        <td id="L36" class="blob-num js-line-number" data-line-number="36"></td>
        <td id="LC36" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span> <span class="pl-k">=</span> {</td>
      </tr>
      <tr>
        <td id="L37" class="blob-num js-line-number" data-line-number="37"></td>
        <td id="LC37" class="blob-code blob-code-inner js-file-line">		name<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>jquery.mb.vimeo_player<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L38" class="blob-num js-line-number" data-line-number="38"></td>
        <td id="LC38" class="blob-code blob-code-inner js-file-line">		author<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>Matteo Bicocchi (pupunzi)<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L39" class="blob-num js-line-number" data-line-number="39"></td>
        <td id="LC39" class="blob-code blob-code-inner js-file-line">		version<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>1.0.8<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L40" class="blob-num js-line-number" data-line-number="40"></td>
        <td id="LC40" class="blob-code blob-code-inner js-file-line">		build<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>395<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L41" class="blob-num js-line-number" data-line-number="41"></td>
        <td id="LC41" class="blob-code blob-code-inner js-file-line">		defaults<span class="pl-k">:</span> {</td>
      </tr>
      <tr>
        <td id="L42" class="blob-num js-line-number" data-line-number="42"></td>
        <td id="LC42" class="blob-code blob-code-inner js-file-line">			containment<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>body<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L43" class="blob-num js-line-number" data-line-number="43"></td>
        <td id="LC43" class="blob-code blob-code-inner js-file-line">			ratio<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>16/9<span class="pl-pds">&quot;</span></span>, <span class="pl-c"><span class="pl-c">//</span> &quot;16/9&quot; or &quot;4/3&quot;</span></td>
      </tr>
      <tr>
        <td id="L44" class="blob-num js-line-number" data-line-number="44"></td>
        <td id="LC44" class="blob-code blob-code-inner js-file-line">			videoURL<span class="pl-k">:</span> <span class="pl-c1">null</span>,</td>
      </tr>
      <tr>
        <td id="L45" class="blob-num js-line-number" data-line-number="45"></td>
        <td id="LC45" class="blob-code blob-code-inner js-file-line">			startAt<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L46" class="blob-num js-line-number" data-line-number="46"></td>
        <td id="LC46" class="blob-code blob-code-inner js-file-line">			stopAt<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L47" class="blob-num js-line-number" data-line-number="47"></td>
        <td id="LC47" class="blob-code blob-code-inner js-file-line">			autoPlay<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L48" class="blob-num js-line-number" data-line-number="48"></td>
        <td id="LC48" class="blob-code blob-code-inner js-file-line">			fadeTime<span class="pl-k">:</span> <span class="pl-c1">1000</span>,</td>
      </tr>
      <tr>
        <td id="L49" class="blob-num js-line-number" data-line-number="49"></td>
        <td id="LC49" class="blob-code blob-code-inner js-file-line">			vol<span class="pl-k">:</span> <span class="pl-c1">50</span>, <span class="pl-c"><span class="pl-c">//</span> 1 to 100</span></td>
      </tr>
      <tr>
        <td id="L50" class="blob-num js-line-number" data-line-number="50"></td>
        <td id="LC50" class="blob-code blob-code-inner js-file-line">			addRaster<span class="pl-k">:</span> <span class="pl-c1">false</span>,</td>
      </tr>
      <tr>
        <td id="L51" class="blob-num js-line-number" data-line-number="51"></td>
        <td id="LC51" class="blob-code blob-code-inner js-file-line">			opacity<span class="pl-k">:</span> <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L52" class="blob-num js-line-number" data-line-number="52"></td>
        <td id="LC52" class="blob-code blob-code-inner js-file-line">			mute<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L53" class="blob-num js-line-number" data-line-number="53"></td>
        <td id="LC53" class="blob-code blob-code-inner js-file-line">			loop<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L54" class="blob-num js-line-number" data-line-number="54"></td>
        <td id="LC54" class="blob-code blob-code-inner js-file-line">			showControls<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L55" class="blob-num js-line-number" data-line-number="55"></td>
        <td id="LC55" class="blob-code blob-code-inner js-file-line">			show_vimeo_logo<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L56" class="blob-num js-line-number" data-line-number="56"></td>
        <td id="LC56" class="blob-code blob-code-inner js-file-line">			stopMovieOnBlur<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L57" class="blob-num js-line-number" data-line-number="57"></td>
        <td id="LC57" class="blob-code blob-code-inner js-file-line">			realfullscreen<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L58" class="blob-num js-line-number" data-line-number="58"></td>
        <td id="LC58" class="blob-code blob-code-inner js-file-line">			mobileFallbackImage<span class="pl-k">:</span> <span class="pl-c1">null</span>,</td>
      </tr>
      <tr>
        <td id="L59" class="blob-num js-line-number" data-line-number="59"></td>
        <td id="LC59" class="blob-code blob-code-inner js-file-line">			gaTrack<span class="pl-k">:</span> <span class="pl-c1">false</span>,</td>
      </tr>
      <tr>
        <td id="L60" class="blob-num js-line-number" data-line-number="60"></td>
        <td id="LC60" class="blob-code blob-code-inner js-file-line">			optimizeDisplay<span class="pl-k">:</span> <span class="pl-c1">true</span>,</td>
      </tr>
      <tr>
        <td id="L61" class="blob-num js-line-number" data-line-number="61"></td>
        <td id="LC61" class="blob-code blob-code-inner js-file-line">			mask<span class="pl-k">:</span> <span class="pl-c1">false</span>,</td>
      </tr>
      <tr>
        <td id="L62" class="blob-num js-line-number" data-line-number="62"></td>
        <td id="LC62" class="blob-code blob-code-inner js-file-line">			align<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>center,center<span class="pl-pds">&quot;</span></span>, <span class="pl-c"><span class="pl-c">//</span> top,bottom,left,right</span></td>
      </tr>
      <tr>
        <td id="L63" class="blob-num js-line-number" data-line-number="63"></td>
        <td id="LC63" class="blob-code blob-code-inner js-file-line">			<span class="pl-en">onReady</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">player</span> ) {}</td>
      </tr>
      <tr>
        <td id="L64" class="blob-num js-line-number" data-line-number="64"></td>
        <td id="LC64" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L65" class="blob-num js-line-number" data-line-number="65"></td>
        <td id="LC65" class="blob-code blob-code-inner js-file-line">		<span class="pl-c">/**</span></td>
      </tr>
      <tr>
        <td id="L66" class="blob-num js-line-number" data-line-number="66"></td>
        <td id="LC66" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 *  @fontface icons</span></td>
      </tr>
      <tr>
        <td id="L67" class="blob-num js-line-number" data-line-number="67"></td>
        <td id="LC67" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 *  */</span></td>
      </tr>
      <tr>
        <td id="L68" class="blob-num js-line-number" data-line-number="68"></td>
        <td id="LC68" class="blob-code blob-code-inner js-file-line">		controls<span class="pl-k">:</span> {</td>
      </tr>
      <tr>
        <td id="L69" class="blob-num js-line-number" data-line-number="69"></td>
        <td id="LC69" class="blob-code blob-code-inner js-file-line">			play<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>P<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L70" class="blob-num js-line-number" data-line-number="70"></td>
        <td id="LC70" class="blob-code blob-code-inner js-file-line">			pause<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>p<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L71" class="blob-num js-line-number" data-line-number="71"></td>
        <td id="LC71" class="blob-code blob-code-inner js-file-line">			mute<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>M<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L72" class="blob-num js-line-number" data-line-number="72"></td>
        <td id="LC72" class="blob-code blob-code-inner js-file-line">			unmute<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>A<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L73" class="blob-num js-line-number" data-line-number="73"></td>
        <td id="LC73" class="blob-code blob-code-inner js-file-line">			fullscreen<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>O<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L74" class="blob-num js-line-number" data-line-number="74"></td>
        <td id="LC74" class="blob-code blob-code-inner js-file-line">			showSite<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>R<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L75" class="blob-num js-line-number" data-line-number="75"></td>
        <td id="LC75" class="blob-code blob-code-inner js-file-line">			logo<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>V<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L76" class="blob-num js-line-number" data-line-number="76"></td>
        <td id="LC76" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L77" class="blob-num js-line-number" data-line-number="77"></td>
        <td id="LC77" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">buildPlayer</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">options</span> ) {</td>
      </tr>
      <tr>
        <td id="L78" class="blob-num js-line-number" data-line-number="78"></td>
        <td id="LC78" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L79" class="blob-num js-line-number" data-line-number="79"></td>
        <td id="LC79" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> <span class="pl-en">isIframe</span> <span class="pl-k">=</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L80" class="blob-num js-line-number" data-line-number="80"></td>
        <td id="LC80" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> isIfr <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L81" class="blob-num js-line-number" data-line-number="81"></td>
        <td id="LC81" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">try</span> {</td>
      </tr>
      <tr>
        <td id="L82" class="blob-num js-line-number" data-line-number="82"></td>
        <td id="LC82" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-smi">self</span>.<span class="pl-c1">location</span>.<span class="pl-c1">href</span> <span class="pl-k">!=</span> <span class="pl-smi">top</span>.<span class="pl-c1">location</span>.<span class="pl-c1">href</span> ) isIfr <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L83" class="blob-num js-line-number" data-line-number="83"></td>
        <td id="LC83" class="blob-code blob-code-inner js-file-line">				} <span class="pl-k">catch</span>( e ) {</td>
      </tr>
      <tr>
        <td id="L84" class="blob-num js-line-number" data-line-number="84"></td>
        <td id="LC84" class="blob-code blob-code-inner js-file-line">					isIfr <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L85" class="blob-num js-line-number" data-line-number="85"></td>
        <td id="LC85" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L86" class="blob-num js-line-number" data-line-number="86"></td>
        <td id="LC86" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span> isIfr;</td>
      </tr>
      <tr>
        <td id="L87" class="blob-num js-line-number" data-line-number="87"></td>
        <td id="LC87" class="blob-code blob-code-inner js-file-line">			};</td>
      </tr>
      <tr>
        <td id="L88" class="blob-num js-line-number" data-line-number="88"></td>
        <td id="LC88" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L89" class="blob-num js-line-number" data-line-number="89"></td>
        <td id="LC89" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L90" class="blob-num js-line-number" data-line-number="90"></td>
        <td id="LC90" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> script <span class="pl-k">=</span> <span class="pl-c1">document</span>.<span class="pl-c1">createElement</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>script<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L91" class="blob-num js-line-number" data-line-number="91"></td>
        <td id="LC91" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">script</span>.<span class="pl-smi">src</span> <span class="pl-k">=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>//player.vimeo.com/api/player.js<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L92" class="blob-num js-line-number" data-line-number="92"></td>
        <td id="LC92" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">script</span>.<span class="pl-en">onload</span> <span class="pl-k">=</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L93" class="blob-num js-line-number" data-line-number="93"></td>
        <td id="LC93" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-c1">document</span> ).<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_api_loaded<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L94" class="blob-num js-line-number" data-line-number="94"></td>
        <td id="LC94" class="blob-code blob-code-inner js-file-line">			};</td>
      </tr>
      <tr>
        <td id="L95" class="blob-num js-line-number" data-line-number="95"></td>
        <td id="LC95" class="blob-code blob-code-inner js-file-line">			<span class="pl-c1">document</span>.<span class="pl-smi">head</span>.<span class="pl-c1">appendChild</span>( script );</td>
      </tr>
      <tr>
        <td id="L96" class="blob-num js-line-number" data-line-number="96"></td>
        <td id="LC96" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L97" class="blob-num js-line-number" data-line-number="97"></td>
        <td id="LC97" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>.<span class="pl-en">each</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L98" class="blob-num js-line-number" data-line-number="98"></td>
        <td id="LC98" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L99" class="blob-num js-line-number" data-line-number="99"></td>
        <td id="LC99" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L100" class="blob-num js-line-number" data-line-number="100"></td>
        <td id="LC100" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> $vimeo_player <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( vimeo_player );</td>
      </tr>
      <tr>
        <td id="L101" class="blob-num js-line-number" data-line-number="101"></td>
        <td id="LC101" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">loop</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L102" class="blob-num js-line-number" data-line-number="102"></td>
        <td id="LC102" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span> <span class="pl-k">=</span> {};</td>
      </tr>
      <tr>
        <td id="L103" class="blob-num js-line-number" data-line-number="103"></td>
        <td id="LC103" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> {};</td>
      </tr>
      <tr>
        <td id="L104" class="blob-num js-line-number" data-line-number="104"></td>
        <td id="LC104" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> <span class="pl-k">||</span> <span class="pl-s"><span class="pl-pds">&quot;</span>YTP_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-k">new</span> <span class="pl-en">Date</span>().<span class="pl-c1">getTime</span>();</td>
      </tr>
      <tr>
        <td id="L105" class="blob-num js-line-number" data-line-number="105"></td>
        <td id="LC105" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L106" class="blob-num js-line-number" data-line-number="106"></td>
        <td id="LC106" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L107" class="blob-num js-line-number" data-line-number="107"></td>
        <td id="LC107" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> property <span class="pl-k">=</span> <span class="pl-smi">$vimeo_player</span>.<span class="pl-c1">data</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>property<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">&amp;&amp;</span> <span class="pl-k">typeof</span> <span class="pl-smi">$vimeo_player</span>.<span class="pl-c1">data</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>property<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>string<span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> <span class="pl-c1">eval</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>(<span class="pl-pds">&#39;</span></span> <span class="pl-k">+</span> <span class="pl-smi">$vimeo_player</span>.<span class="pl-c1">data</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>property<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&#39;</span>)<span class="pl-pds">&#39;</span></span> ) <span class="pl-k">:</span> <span class="pl-smi">$vimeo_player</span>.<span class="pl-c1">data</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>property<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L108" class="blob-num js-line-number" data-line-number="108"></td>
        <td id="LC108" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L109" class="blob-num js-line-number" data-line-number="109"></td>
        <td id="LC109" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">jQuery</span>.<span class="pl-en">extend</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>, <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">defaults</span>, options, property );</td>
      </tr>
      <tr>
        <td id="L110" class="blob-num js-line-number" data-line-number="110"></td>
        <td id="LC110" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L111" class="blob-num js-line-number" data-line-number="111"></td>
        <td id="LC111" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>16/9<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span>;</td>
      </tr>
      <tr>
        <td id="L112" class="blob-num js-line-number" data-line-number="112"></td>
        <td id="LC112" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L113" class="blob-num js-line-number" data-line-number="113"></td>
        <td id="LC113" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-c1">eval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">loop</span> ) )</td>
      </tr>
      <tr>
        <td id="L114" class="blob-num js-line-number" data-line-number="114"></td>
        <td id="LC114" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">loop</span> <span class="pl-k">=</span> <span class="pl-c1">9999</span>;</td>
      </tr>
      <tr>
        <td id="L115" class="blob-num js-line-number" data-line-number="115"></td>
        <td id="LC115" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L116" class="blob-num js-line-number" data-line-number="116"></td>
        <td id="LC116" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isRetina</span> <span class="pl-k">=</span> ( <span class="pl-c1">window</span>.<span class="pl-smi">retina</span> <span class="pl-k">||</span> <span class="pl-c1">window</span>.<span class="pl-smi">devicePixelRatio</span> <span class="pl-k">&gt;</span> <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L117" class="blob-num js-line-number" data-line-number="117"></td>
        <td id="LC117" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L118" class="blob-num js-line-number" data-line-number="118"></td>
        <td id="LC118" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L119" class="blob-num js-line-number" data-line-number="119"></td>
        <td id="LC119" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">canGoFullScreen</span> <span class="pl-k">=</span> <span class="pl-k">!</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">msie</span> <span class="pl-k">||</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">opera</span> <span class="pl-k">||</span> <span class="pl-en">isIframe</span>() );</td>
      </tr>
      <tr>
        <td id="L120" class="blob-num js-line-number" data-line-number="120"></td>
        <td id="LC120" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">canGoFullScreen</span> ) <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">realfullscreen</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L121" class="blob-num js-line-number" data-line-number="121"></td>
        <td id="LC121" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L122" class="blob-num js-line-number" data-line-number="122"></td>
        <td id="LC122" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isAlone</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L123" class="blob-num js-line-number" data-line-number="123"></td>
        <td id="LC123" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-c1">hasFocus</span> <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L124" class="blob-num js-line-number" data-line-number="124"></td>
        <td id="LC124" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L125" class="blob-num js-line-number" data-line-number="125"></td>
        <td id="LC125" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">videoID</span> <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">videoURL</span> <span class="pl-k">?</span> <span class="pl-en">get_vimeo_videoID</span>( <span class="pl-c1">this</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">videoURL</span> ) <span class="pl-k">:</span> <span class="pl-smi">$vimeo_player</span>.<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>href<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">?</span> <span class="pl-en">get_vimeo_videoID</span>( <span class="pl-smi">$vimeo_player</span>.<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>href<span class="pl-pds">&quot;</span></span> ) ) <span class="pl-k">:</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L126" class="blob-num js-line-number" data-line-number="126"></td>
        <td id="LC126" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L127" class="blob-num js-line-number" data-line-number="127"></td>
        <td id="LC127" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isSelf</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span> <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>self<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L128" class="blob-num js-line-number" data-line-number="128"></td>
        <td id="LC128" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span> <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>self<span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> <span class="pl-en">jQuery</span>( <span class="pl-c1">this</span> ) <span class="pl-k">:</span> <span class="pl-en">jQuery</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span> );</td>
      </tr>
      <tr>
        <td id="L129" class="blob-num js-line-number" data-line-number="129"></td>
        <td id="LC129" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span>.<span class="pl-en">is</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>body<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L130" class="blob-num js-line-number" data-line-number="130"></td>
        <td id="LC130" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L131" class="blob-num js-line-number" data-line-number="131"></td>
        <td id="LC131" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">backgroundIsInited</span> )</td>
      </tr>
      <tr>
        <td id="L132" class="blob-num js-line-number" data-line-number="132"></td>
        <td id="LC132" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L133" class="blob-num js-line-number" data-line-number="133"></td>
        <td id="LC133" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L134" class="blob-num js-line-number" data-line-number="134"></td>
        <td id="LC134" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">canPlayOnMobile</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isSelf</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-en">jQuery</span>( <span class="pl-c1">this</span> ).<span class="pl-en">children</span>().<span class="pl-c1">length</span> <span class="pl-k">===</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L135" class="blob-num js-line-number" data-line-number="135"></td>
        <td id="LC135" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L136" class="blob-num js-line-number" data-line-number="136"></td>
        <td id="LC136" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isSelf</span> ) {</td>
      </tr>
      <tr>
        <td id="L137" class="blob-num js-line-number" data-line-number="137"></td>
        <td id="LC137" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">hide</span>();</td>
      </tr>
      <tr>
        <td id="L138" class="blob-num js-line-number" data-line-number="138"></td>
        <td id="LC138" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L139" class="blob-num js-line-number" data-line-number="139"></td>
        <td id="LC139" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L140" class="blob-num js-line-number" data-line-number="140"></td>
        <td id="LC140" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> overlay <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L141" class="blob-num js-line-number" data-line-number="141"></td>
        <td id="LC141" class="blob-code blob-code-inner js-file-line">					position<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L142" class="blob-num js-line-number" data-line-number="142"></td>
        <td id="LC142" class="blob-code blob-code-inner js-file-line">					top<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L143" class="blob-num js-line-number" data-line-number="143"></td>
        <td id="LC143" class="blob-code blob-code-inner js-file-line">					left<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L144" class="blob-num js-line-number" data-line-number="144"></td>
        <td id="LC144" class="blob-code blob-code-inner js-file-line">					width<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L145" class="blob-num js-line-number" data-line-number="145"></td>
        <td id="LC145" class="blob-code blob-code-inner js-file-line">					height<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L146" class="blob-num js-line-number" data-line-number="146"></td>
        <td id="LC146" class="blob-code blob-code-inner js-file-line">				} ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_overlay<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L147" class="blob-num js-line-number" data-line-number="147"></td>
        <td id="LC147" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L148" class="blob-num js-line-number" data-line-number="148"></td>
        <td id="LC148" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isSelf</span> ) {</td>
      </tr>
      <tr>
        <td id="L149" class="blob-num js-line-number" data-line-number="149"></td>
        <td id="LC149" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">overlay</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>click<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L150" class="blob-num js-line-number" data-line-number="150"></td>
        <td id="LC150" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">togglePlay</span>();</td>
      </tr>
      <tr>
        <td id="L151" class="blob-num js-line-number" data-line-number="151"></td>
        <td id="LC151" class="blob-code blob-code-inner js-file-line">					} )</td>
      </tr>
      <tr>
        <td id="L152" class="blob-num js-line-number" data-line-number="152"></td>
        <td id="LC152" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L153" class="blob-num js-line-number" data-line-number="153"></td>
        <td id="LC153" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L154" class="blob-num js-line-number" data-line-number="154"></td>
        <td id="LC154" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> playerID <span class="pl-k">=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span>;</td>
      </tr>
      <tr>
        <td id="L155" class="blob-num js-line-number" data-line-number="155"></td>
        <td id="LC155" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L156" class="blob-num js-line-number" data-line-number="156"></td>
        <td id="LC156" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> wrapper <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_wrapper<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>id<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_wrapper_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> playerID );</td>
      </tr>
      <tr>
        <td id="L157" class="blob-num js-line-number" data-line-number="157"></td>
        <td id="LC157" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">wrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L158" class="blob-num js-line-number" data-line-number="158"></td>
        <td id="LC158" class="blob-code blob-code-inner js-file-line">					position<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L159" class="blob-num js-line-number" data-line-number="159"></td>
        <td id="LC159" class="blob-code blob-code-inner js-file-line">					zIndex<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L160" class="blob-num js-line-number" data-line-number="160"></td>
        <td id="LC160" class="blob-code blob-code-inner js-file-line">					minWidth<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L161" class="blob-num js-line-number" data-line-number="161"></td>
        <td id="LC161" class="blob-code blob-code-inner js-file-line">					minHeight<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L162" class="blob-num js-line-number" data-line-number="162"></td>
        <td id="LC162" class="blob-code blob-code-inner js-file-line">					left<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L163" class="blob-num js-line-number" data-line-number="163"></td>
        <td id="LC163" class="blob-code blob-code-inner js-file-line">					top<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L164" class="blob-num js-line-number" data-line-number="164"></td>
        <td id="LC164" class="blob-code blob-code-inner js-file-line">					overflow<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>hidden<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L165" class="blob-num js-line-number" data-line-number="165"></td>
        <td id="LC165" class="blob-code blob-code-inner js-file-line">					opacity<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L166" class="blob-num js-line-number" data-line-number="166"></td>
        <td id="LC166" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L167" class="blob-num js-line-number" data-line-number="167"></td>
        <td id="LC167" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L168" class="blob-num js-line-number" data-line-number="168"></td>
        <td id="LC168" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span> <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;iframe/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>id<span class="pl-pds">&quot;</span></span>, playerID ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>playerBox<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L169" class="blob-num js-line-number" data-line-number="169"></td>
        <td id="LC169" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L170" class="blob-num js-line-number" data-line-number="170"></td>
        <td id="LC170" class="blob-code blob-code-inner js-file-line">					position<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L171" class="blob-num js-line-number" data-line-number="171"></td>
        <td id="LC171" class="blob-code blob-code-inner js-file-line">					zIndex<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L172" class="blob-num js-line-number" data-line-number="172"></td>
        <td id="LC172" class="blob-code blob-code-inner js-file-line">					width<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L173" class="blob-num js-line-number" data-line-number="173"></td>
        <td id="LC173" class="blob-code blob-code-inner js-file-line">					height<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L174" class="blob-num js-line-number" data-line-number="174"></td>
        <td id="LC174" class="blob-code blob-code-inner js-file-line">					top<span class="pl-k">:</span> <span class="pl-k">-</span><span class="pl-c1">10</span>,</td>
      </tr>
      <tr>
        <td id="L175" class="blob-num js-line-number" data-line-number="175"></td>
        <td id="LC175" class="blob-code blob-code-inner js-file-line">					frameBorder<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L176" class="blob-num js-line-number" data-line-number="176"></td>
        <td id="LC176" class="blob-code blob-code-inner js-file-line">					overflow<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>hidden<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L177" class="blob-num js-line-number" data-line-number="177"></td>
        <td id="LC177" class="blob-code blob-code-inner js-file-line">					left<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L178" class="blob-num js-line-number" data-line-number="178"></td>
        <td id="LC178" class="blob-code blob-code-inner js-file-line">				} ).<span class="pl-en">attr</span>( {</td>
      </tr>
      <tr>
        <td id="L179" class="blob-num js-line-number" data-line-number="179"></td>
        <td id="LC179" class="blob-code blob-code-inner js-file-line">					src<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>//player.vimeo.com/video/<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">videoID</span> <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span>?background=1<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L180" class="blob-num js-line-number" data-line-number="180"></td>
        <td id="LC180" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L181" class="blob-num js-line-number" data-line-number="181"></td>
        <td id="LC181" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L182" class="blob-num js-line-number" data-line-number="182"></td>
        <td id="LC182" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span> <span class="pl-k">||</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">canPlayOnMobile</span> )</td>
      </tr>
      <tr>
        <td id="L183" class="blob-num js-line-number" data-line-number="183"></td>
        <td id="LC183" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">wrapper</span>.<span class="pl-c1">append</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span> );</td>
      </tr>
      <tr>
        <td id="L184" class="blob-num js-line-number" data-line-number="184"></td>
        <td id="LC184" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L185" class="blob-num js-line-number" data-line-number="185"></td>
        <td id="LC185" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">mobileFallbackImage</span> ) {</td>
      </tr>
      <tr>
        <td id="L186" class="blob-num js-line-number" data-line-number="186"></td>
        <td id="LC186" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">wrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L187" class="blob-num js-line-number" data-line-number="187"></td>
        <td id="LC187" class="blob-code blob-code-inner js-file-line">							backgroundImage<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>url(<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">mobileFallbackImage</span> <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span>)<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L188" class="blob-num js-line-number" data-line-number="188"></td>
        <td id="LC188" class="blob-code blob-code-inner js-file-line">							backgroundPosition<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>center center<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L189" class="blob-num js-line-number" data-line-number="189"></td>
        <td id="LC189" class="blob-code blob-code-inner js-file-line">							backgroundSize<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>cover<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L190" class="blob-num js-line-number" data-line-number="190"></td>
        <td id="LC190" class="blob-code blob-code-inner js-file-line">							backgroundRepeat<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>no-repeat<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L191" class="blob-num js-line-number" data-line-number="191"></td>
        <td id="LC191" class="blob-code blob-code-inner js-file-line">							opacity<span class="pl-k">:</span> <span class="pl-c1">1</span></td>
      </tr>
      <tr>
        <td id="L192" class="blob-num js-line-number" data-line-number="192"></td>
        <td id="LC192" class="blob-code blob-code-inner js-file-line">						} )</td>
      </tr>
      <tr>
        <td id="L193" class="blob-num js-line-number" data-line-number="193"></td>
        <td id="LC193" class="blob-code blob-code-inner js-file-line">					};</td>
      </tr>
      <tr>
        <td id="L194" class="blob-num js-line-number" data-line-number="194"></td>
        <td id="LC194" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L195" class="blob-num js-line-number" data-line-number="195"></td>
        <td id="LC195" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">$vimeo_player</span>.<span class="pl-c1">remove</span>();</td>
      </tr>
      <tr>
        <td id="L196" class="blob-num js-line-number" data-line-number="196"></td>
        <td id="LC196" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L197" class="blob-num js-line-number" data-line-number="197"></td>
        <td id="LC197" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L198" class="blob-num js-line-number" data-line-number="198"></td>
        <td id="LC198" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L199" class="blob-num js-line-number" data-line-number="199"></td>
        <td id="LC199" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span>.<span class="pl-en">children</span>().<span class="pl-en">not</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>script, style<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">each</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L200" class="blob-num js-line-number" data-line-number="200"></td>
        <td id="LC200" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-en">jQuery</span>( <span class="pl-c1">this</span> ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>static<span class="pl-pds">&quot;</span></span> ) <span class="pl-en">jQuery</span>( <span class="pl-c1">this</span> ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>relative<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L201" class="blob-num js-line-number" data-line-number="201"></td>
        <td id="LC201" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L202" class="blob-num js-line-number" data-line-number="202"></td>
        <td id="LC202" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L203" class="blob-num js-line-number" data-line-number="203"></td>
        <td id="LC203" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> ) {</td>
      </tr>
      <tr>
        <td id="L204" class="blob-num js-line-number" data-line-number="204"></td>
        <td id="LC204" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>body<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L205" class="blob-num js-line-number" data-line-number="205"></td>
        <td id="LC205" class="blob-code blob-code-inner js-file-line">						boxSizing<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>border-box<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L206" class="blob-num js-line-number" data-line-number="206"></td>
        <td id="LC206" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L207" class="blob-num js-line-number" data-line-number="207"></td>
        <td id="LC207" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L208" class="blob-num js-line-number" data-line-number="208"></td>
        <td id="LC208" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">wrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L209" class="blob-num js-line-number" data-line-number="209"></td>
        <td id="LC209" class="blob-code blob-code-inner js-file-line">						position<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>fixed<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L210" class="blob-num js-line-number" data-line-number="210"></td>
        <td id="LC210" class="blob-code blob-code-inner js-file-line">						top<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L211" class="blob-num js-line-number" data-line-number="211"></td>
        <td id="LC211" class="blob-code blob-code-inner js-file-line">						left<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L212" class="blob-num js-line-number" data-line-number="212"></td>
        <td id="LC212" class="blob-code blob-code-inner js-file-line">						zIndex<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L213" class="blob-num js-line-number" data-line-number="213"></td>
        <td id="LC213" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L214" class="blob-num js-line-number" data-line-number="214"></td>
        <td id="LC214" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L215" class="blob-num js-line-number" data-line-number="215"></td>
        <td id="LC215" class="blob-code blob-code-inner js-file-line">				} <span class="pl-k">else</span> <span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span>.<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>static<span class="pl-pds">&quot;</span></span> )</td>
      </tr>
      <tr>
        <td id="L216" class="blob-num js-line-number" data-line-number="216"></td>
        <td id="LC216" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L217" class="blob-num js-line-number" data-line-number="217"></td>
        <td id="LC217" class="blob-code blob-code-inner js-file-line">						position<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>relative<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L218" class="blob-num js-line-number" data-line-number="218"></td>
        <td id="LC218" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L219" class="blob-num js-line-number" data-line-number="219"></td>
        <td id="LC219" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L220" class="blob-num js-line-number" data-line-number="220"></td>
        <td id="LC220" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span>.<span class="pl-c1">prepend</span>( wrapper );</td>
      </tr>
      <tr>
        <td id="L221" class="blob-num js-line-number" data-line-number="221"></td>
        <td id="LC221" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span> <span class="pl-k">=</span> wrapper;</td>
      </tr>
      <tr>
        <td id="L222" class="blob-num js-line-number" data-line-number="222"></td>
        <td id="LC222" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L223" class="blob-num js-line-number" data-line-number="223"></td>
        <td id="LC223" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L224" class="blob-num js-line-number" data-line-number="224"></td>
        <td id="LC224" class="blob-code blob-code-inner js-file-line">					opacity<span class="pl-k">:</span> <span class="pl-c1">1</span></td>
      </tr>
      <tr>
        <td id="L225" class="blob-num js-line-number" data-line-number="225"></td>
        <td id="LC225" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L226" class="blob-num js-line-number" data-line-number="226"></td>
        <td id="LC226" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L227" class="blob-num js-line-number" data-line-number="227"></td>
        <td id="LC227" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span> ) {</td>
      </tr>
      <tr>
        <td id="L228" class="blob-num js-line-number" data-line-number="228"></td>
        <td id="LC228" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span>.<span class="pl-c1">after</span>( overlay );</td>
      </tr>
      <tr>
        <td id="L229" class="blob-num js-line-number" data-line-number="229"></td>
        <td id="LC229" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span> <span class="pl-k">=</span> overlay;</td>
      </tr>
      <tr>
        <td id="L230" class="blob-num js-line-number" data-line-number="230"></td>
        <td id="LC230" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L231" class="blob-num js-line-number" data-line-number="231"></td>
        <td id="LC231" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L232" class="blob-num js-line-number" data-line-number="232"></td>
        <td id="LC232" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> ) {</td>
      </tr>
      <tr>
        <td id="L233" class="blob-num js-line-number" data-line-number="233"></td>
        <td id="LC233" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">overlay</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>mouseenter<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L234" class="blob-num js-line-number" data-line-number="234"></td>
        <td id="LC234" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L235" class="blob-num js-line-number" data-line-number="235"></td>
        <td id="LC235" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>visible<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L236" class="blob-num js-line-number" data-line-number="236"></td>
        <td id="LC236" class="blob-code blob-code-inner js-file-line">					} ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>mouseleave<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L237" class="blob-num js-line-number" data-line-number="237"></td>
        <td id="LC237" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L238" class="blob-num js-line-number" data-line-number="238"></td>
        <td id="LC238" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-en">removeClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>visible<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L239" class="blob-num js-line-number" data-line-number="239"></td>
        <td id="LC239" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L240" class="blob-num js-line-number" data-line-number="240"></td>
        <td id="LC240" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L241" class="blob-num js-line-number" data-line-number="241"></td>
        <td id="LC241" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L242" class="blob-num js-line-number" data-line-number="242"></td>
        <td id="LC242" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-c1">document</span> ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_api_loaded<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L243" class="blob-num js-line-number" data-line-number="243"></td>
        <td id="LC243" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L244" class="blob-num js-line-number" data-line-number="244"></td>
        <td id="LC244" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span> <span class="pl-k">=</span> <span class="pl-k">new</span> <span class="pl-en">Vimeo.Player</span>( playerID, options );</td>
      </tr>
      <tr>
        <td id="L245" class="blob-num js-line-number" data-line-number="245"></td>
        <td id="LC245" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">ready</span>().<span class="pl-en">then</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L246" class="blob-num js-line-number" data-line-number="246"></td>
        <td id="LC246" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L247" class="blob-num js-line-number" data-line-number="247"></td>
        <td id="LC247" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">var</span> VEvent;</td>
      </tr>
      <tr>
        <td id="L248" class="blob-num js-line-number" data-line-number="248"></td>
        <td id="LC248" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L249" class="blob-num js-line-number" data-line-number="249"></td>
        <td id="LC249" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">function</span> <span class="pl-en">start</span>() {</td>
      </tr>
      <tr>
        <td id="L250" class="blob-num js-line-number" data-line-number="250"></td>
        <td id="LC250" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isReady</span> <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L251" class="blob-num js-line-number" data-line-number="251"></td>
        <td id="LC251" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">mute</span> ) {</td>
      </tr>
      <tr>
        <td id="L252" class="blob-num js-line-number" data-line-number="252"></td>
        <td id="LC252" class="blob-code blob-code-inner js-file-line">								<span class="pl-c1">setTimeout</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L253" class="blob-num js-line-number" data-line-number="253"></td>
        <td id="LC253" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_mute</span>();</td>
      </tr>
      <tr>
        <td id="L254" class="blob-num js-line-number" data-line-number="254"></td>
        <td id="LC254" class="blob-code blob-code-inner js-file-line">								}, <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L255" class="blob-num js-line-number" data-line-number="255"></td>
        <td id="LC255" class="blob-code blob-code-inner js-file-line">							}</td>
      </tr>
      <tr>
        <td id="L256" class="blob-num js-line-number" data-line-number="256"></td>
        <td id="LC256" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L257" class="blob-num js-line-number" data-line-number="257"></td>
        <td id="LC257" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">showControls</span> )</td>
      </tr>
      <tr>
        <td id="L258" class="blob-num js-line-number" data-line-number="258"></td>
        <td id="LC258" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-en">buildControls</span>( vimeo_player );</td>
      </tr>
      <tr>
        <td id="L259" class="blob-num js-line-number" data-line-number="259"></td>
        <td id="LC259" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L260" class="blob-num js-line-number" data-line-number="260"></td>
        <td id="LC260" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">autoPlay</span> )</td>
      </tr>
      <tr>
        <td id="L261" class="blob-num js-line-number" data-line-number="261"></td>
        <td id="LC261" class="blob-code blob-code-inner js-file-line">								<span class="pl-c1">setTimeout</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L262" class="blob-num js-line-number" data-line-number="262"></td>
        <td id="LC262" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L263" class="blob-num js-line-number" data-line-number="263"></td>
        <td id="LC263" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_play</span>();</td>
      </tr>
      <tr>
        <td id="L264" class="blob-num js-line-number" data-line-number="264"></td>
        <td id="LC264" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L265" class="blob-num js-line-number" data-line-number="265"></td>
        <td id="LC265" class="blob-code blob-code-inner js-file-line">									<span class="pl-c1">setTimeout</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L266" class="blob-num js-line-number" data-line-number="266"></td>
        <td id="LC266" class="blob-code blob-code-inner js-file-line">										VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPStart<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L267" class="blob-num js-line-number" data-line-number="267"></td>
        <td id="LC267" class="blob-code blob-code-inner js-file-line">										<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L268" class="blob-num js-line-number" data-line-number="268"></td>
        <td id="LC268" class="blob-code blob-code-inner js-file-line">									}, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span> )</td>
      </tr>
      <tr>
        <td id="L269" class="blob-num js-line-number" data-line-number="269"></td>
        <td id="LC269" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L270" class="blob-num js-line-number" data-line-number="270"></td>
        <td id="LC270" class="blob-code blob-code-inner js-file-line">								}, <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L271" class="blob-num js-line-number" data-line-number="271"></td>
        <td id="LC271" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L272" class="blob-num js-line-number" data-line-number="272"></td>
        <td id="LC272" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_pause</span>();</td>
      </tr>
      <tr>
        <td id="L273" class="blob-num js-line-number" data-line-number="273"></td>
        <td id="LC273" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L274" class="blob-num js-line-number" data-line-number="274"></td>
        <td id="LC274" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPReady<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L275" class="blob-num js-line-number" data-line-number="275"></td>
        <td id="LC275" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L276" class="blob-num js-line-number" data-line-number="276"></td>
        <td id="LC276" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L277" class="blob-num js-line-number" data-line-number="277"></td>
        <td id="LC277" class="blob-code blob-code-inner js-file-line">						}</td>
      </tr>
      <tr>
        <td id="L278" class="blob-num js-line-number" data-line-number="278"></td>
        <td id="LC278" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L279" class="blob-num js-line-number" data-line-number="279"></td>
        <td id="LC279" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">startAt</span> ) {</td>
      </tr>
      <tr>
        <td id="L280" class="blob-num js-line-number" data-line-number="280"></td>
        <td id="LC280" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L281" class="blob-num js-line-number" data-line-number="281"></td>
        <td id="LC281" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L282" class="blob-num js-line-number" data-line-number="282"></td>
        <td id="LC282" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-c1">play</span>().<span class="pl-en">then</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L283" class="blob-num js-line-number" data-line-number="283"></td>
        <td id="LC283" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-c1">pause</span>();</td>
      </tr>
      <tr>
        <td id="L284" class="blob-num js-line-number" data-line-number="284"></td>
        <td id="LC284" class="blob-code blob-code-inner js-file-line">							} );</td>
      </tr>
      <tr>
        <td id="L285" class="blob-num js-line-number" data-line-number="285"></td>
        <td id="LC285" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L286" class="blob-num js-line-number" data-line-number="286"></td>
        <td id="LC286" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_seekTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">startAt</span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L287" class="blob-num js-line-number" data-line-number="287"></td>
        <td id="LC287" class="blob-code blob-code-inner js-file-line">								<span class="pl-en">start</span>()</td>
      </tr>
      <tr>
        <td id="L288" class="blob-num js-line-number" data-line-number="288"></td>
        <td id="LC288" class="blob-code blob-code-inner js-file-line">							} );</td>
      </tr>
      <tr>
        <td id="L289" class="blob-num js-line-number" data-line-number="289"></td>
        <td id="LC289" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L290" class="blob-num js-line-number" data-line-number="290"></td>
        <td id="LC290" class="blob-code blob-code-inner js-file-line">						} <span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L291" class="blob-num js-line-number" data-line-number="291"></td>
        <td id="LC291" class="blob-code blob-code-inner js-file-line">							<span class="pl-en">start</span>();</td>
      </tr>
      <tr>
        <td id="L292" class="blob-num js-line-number" data-line-number="292"></td>
        <td id="LC292" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L293" class="blob-num js-line-number" data-line-number="293"></td>
        <td id="LC293" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_optimize_display</span>();</td>
      </tr>
      <tr>
        <td id="L294" class="blob-num js-line-number" data-line-number="294"></td>
        <td id="LC294" class="blob-code blob-code-inner js-file-line">						<span class="pl-en">jQuery</span>( <span class="pl-c1">window</span> ).<span class="pl-en">off</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>resize.vimeo_player_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>resize.vimeo_player_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L295" class="blob-num js-line-number" data-line-number="295"></td>
        <td id="LC295" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_optimize_display</span>();</td>
      </tr>
      <tr>
        <td id="L296" class="blob-num js-line-number" data-line-number="296"></td>
        <td id="LC296" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L297" class="blob-num js-line-number" data-line-number="297"></td>
        <td id="LC297" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L298" class="blob-num js-line-number" data-line-number="298"></td>
        <td id="LC298" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>PROGRESS</span></td>
      </tr>
      <tr>
        <td id="L299" class="blob-num js-line-number" data-line-number="299"></td>
        <td id="LC299" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>progress<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L300" class="blob-num js-line-number" data-line-number="300"></td>
        <td id="LC300" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPProgress<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L301" class="blob-num js-line-number" data-line-number="301"></td>
        <td id="LC301" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-c1">data</span> <span class="pl-k">=</span> data;</td>
      </tr>
      <tr>
        <td id="L302" class="blob-num js-line-number" data-line-number="302"></td>
        <td id="LC302" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L303" class="blob-num js-line-number" data-line-number="303"></td>
        <td id="LC303" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L304" class="blob-num js-line-number" data-line-number="304"></td>
        <td id="LC304" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span>							console.debug( &quot;1. progress:: &quot;, data );</span></td>
      </tr>
      <tr>
        <td id="L305" class="blob-num js-line-number" data-line-number="305"></td>
        <td id="LC305" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L306" class="blob-num js-line-number" data-line-number="306"></td>
        <td id="LC306" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L307" class="blob-num js-line-number" data-line-number="307"></td>
        <td id="LC307" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L308" class="blob-num js-line-number" data-line-number="308"></td>
        <td id="LC308" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>ERROR</span></td>
      </tr>
      <tr>
        <td id="L309" class="blob-num js-line-number" data-line-number="309"></td>
        <td id="LC309" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>error<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L310" class="blob-num js-line-number" data-line-number="310"></td>
        <td id="LC310" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-k">-</span><span class="pl-c1">1</span>;</td>
      </tr>
      <tr>
        <td id="L311" class="blob-num js-line-number" data-line-number="311"></td>
        <td id="LC311" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span>console.debug( &quot;error:: &quot;, data );</span></td>
      </tr>
      <tr>
        <td id="L312" class="blob-num js-line-number" data-line-number="312"></td>
        <td id="LC312" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span> Trigger state events</span></td>
      </tr>
      <tr>
        <td id="L313" class="blob-num js-line-number" data-line-number="313"></td>
        <td id="LC313" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPError<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L314" class="blob-num js-line-number" data-line-number="314"></td>
        <td id="LC314" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-smi">error</span> <span class="pl-k">=</span> data;</td>
      </tr>
      <tr>
        <td id="L315" class="blob-num js-line-number" data-line-number="315"></td>
        <td id="LC315" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L316" class="blob-num js-line-number" data-line-number="316"></td>
        <td id="LC316" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L317" class="blob-num js-line-number" data-line-number="317"></td>
        <td id="LC317" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L318" class="blob-num js-line-number" data-line-number="318"></td>
        <td id="LC318" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>PLAY</span></td>
      </tr>
      <tr>
        <td id="L319" class="blob-num js-line-number" data-line-number="319"></td>
        <td id="LC319" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>play<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L320" class="blob-num js-line-number" data-line-number="320"></td>
        <td id="LC320" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">1</span>;</td>
      </tr>
      <tr>
        <td id="L321" class="blob-num js-line-number" data-line-number="321"></td>
        <td id="LC321" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L322" class="blob-num js-line-number" data-line-number="322"></td>
        <td id="LC322" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L323" class="blob-num js-line-number" data-line-number="323"></td>
        <td id="LC323" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L324" class="blob-num js-line-number" data-line-number="324"></td>
        <td id="LC324" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_pause<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">pause</span> );</td>
      </tr>
      <tr>
        <td id="L325" class="blob-num js-line-number" data-line-number="325"></td>
        <td id="LC325" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L326" class="blob-num js-line-number" data-line-number="326"></td>
        <td id="LC326" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-k">typeof</span> _gaq <span class="pl-k">!=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>undefined<span class="pl-pds">&quot;</span></span> <span class="pl-k">&amp;&amp;</span> <span class="pl-c1">eval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">gaTrack</span> ) ) <span class="pl-smi">_gaq</span>.<span class="pl-c1">push</span>( [ <span class="pl-s"><span class="pl-pds">&#39;</span>_trackEvent<span class="pl-pds">&#39;</span></span>, <span class="pl-s"><span class="pl-pds">&#39;</span>vimeo_player<span class="pl-pds">&#39;</span></span>, <span class="pl-s"><span class="pl-pds">&#39;</span>Play<span class="pl-pds">&#39;</span></span>, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">videoID</span> ] );</td>
      </tr>
      <tr>
        <td id="L327" class="blob-num js-line-number" data-line-number="327"></td>
        <td id="LC327" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-k">typeof</span> ga <span class="pl-k">!=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>undefined<span class="pl-pds">&quot;</span></span> <span class="pl-k">&amp;&amp;</span> <span class="pl-c1">eval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">gaTrack</span> ) ) <span class="pl-en">ga</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>send<span class="pl-pds">&#39;</span></span>, <span class="pl-s"><span class="pl-pds">&#39;</span>event<span class="pl-pds">&#39;</span></span>, <span class="pl-s"><span class="pl-pds">&#39;</span>vimeo_player<span class="pl-pds">&#39;</span></span>, <span class="pl-s"><span class="pl-pds">&#39;</span>play<span class="pl-pds">&#39;</span></span>, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">videoID</span> );</td>
      </tr>
      <tr>
        <td id="L328" class="blob-num js-line-number" data-line-number="328"></td>
        <td id="LC328" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L329" class="blob-num js-line-number" data-line-number="329"></td>
        <td id="LC329" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span> Trigger state events</span></td>
      </tr>
      <tr>
        <td id="L330" class="blob-num js-line-number" data-line-number="330"></td>
        <td id="LC330" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPPlay<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L331" class="blob-num js-line-number" data-line-number="331"></td>
        <td id="LC331" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-smi">error</span> <span class="pl-k">=</span> data;</td>
      </tr>
      <tr>
        <td id="L332" class="blob-num js-line-number" data-line-number="332"></td>
        <td id="LC332" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L333" class="blob-num js-line-number" data-line-number="333"></td>
        <td id="LC333" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L334" class="blob-num js-line-number" data-line-number="334"></td>
        <td id="LC334" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L335" class="blob-num js-line-number" data-line-number="335"></td>
        <td id="LC335" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L336" class="blob-num js-line-number" data-line-number="336"></td>
        <td id="LC336" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>PAUSE</span></td>
      </tr>
      <tr>
        <td id="L337" class="blob-num js-line-number" data-line-number="337"></td>
        <td id="LC337" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>pause<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L338" class="blob-num js-line-number" data-line-number="338"></td>
        <td id="LC338" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">2</span>;</td>
      </tr>
      <tr>
        <td id="L339" class="blob-num js-line-number" data-line-number="339"></td>
        <td id="LC339" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L340" class="blob-num js-line-number" data-line-number="340"></td>
        <td id="LC340" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L341" class="blob-num js-line-number" data-line-number="341"></td>
        <td id="LC341" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L342" class="blob-num js-line-number" data-line-number="342"></td>
        <td id="LC342" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_pause<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">play</span> );</td>
      </tr>
      <tr>
        <td id="L343" class="blob-num js-line-number" data-line-number="343"></td>
        <td id="LC343" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L344" class="blob-num js-line-number" data-line-number="344"></td>
        <td id="LC344" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPPause<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L345" class="blob-num js-line-number" data-line-number="345"></td>
        <td id="LC345" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-smi">time</span> <span class="pl-k">=</span> data;</td>
      </tr>
      <tr>
        <td id="L346" class="blob-num js-line-number" data-line-number="346"></td>
        <td id="LC346" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L347" class="blob-num js-line-number" data-line-number="347"></td>
        <td id="LC347" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L348" class="blob-num js-line-number" data-line-number="348"></td>
        <td id="LC348" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L349" class="blob-num js-line-number" data-line-number="349"></td>
        <td id="LC349" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L350" class="blob-num js-line-number" data-line-number="350"></td>
        <td id="LC350" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>SEEKED</span></td>
      </tr>
      <tr>
        <td id="L351" class="blob-num js-line-number" data-line-number="351"></td>
        <td id="LC351" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>seeked<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L352" class="blob-num js-line-number" data-line-number="352"></td>
        <td id="LC352" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">3</span>;</td>
      </tr>
      <tr>
        <td id="L353" class="blob-num js-line-number" data-line-number="353"></td>
        <td id="LC353" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span> )</td>
      </tr>
      <tr>
        <td id="L354" class="blob-num js-line-number" data-line-number="354"></td>
        <td id="LC354" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L355" class="blob-num js-line-number" data-line-number="355"></td>
        <td id="LC355" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L356" class="blob-num js-line-number" data-line-number="356"></td>
        <td id="LC356" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>ENDED</span></td>
      </tr>
      <tr>
        <td id="L357" class="blob-num js-line-number" data-line-number="357"></td>
        <td id="LC357" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>ended<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L358" class="blob-num js-line-number" data-line-number="358"></td>
        <td id="LC358" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L359" class="blob-num js-line-number" data-line-number="359"></td>
        <td id="LC359" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L360" class="blob-num js-line-number" data-line-number="360"></td>
        <td id="LC360" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L361" class="blob-num js-line-number" data-line-number="361"></td>
        <td id="LC361" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPEnd<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L362" class="blob-num js-line-number" data-line-number="362"></td>
        <td id="LC362" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-smi">time</span> <span class="pl-k">=</span> data;</td>
      </tr>
      <tr>
        <td id="L363" class="blob-num js-line-number" data-line-number="363"></td>
        <td id="LC363" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L364" class="blob-num js-line-number" data-line-number="364"></td>
        <td id="LC364" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L365" class="blob-num js-line-number" data-line-number="365"></td>
        <td id="LC365" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L366" class="blob-num js-line-number" data-line-number="366"></td>
        <td id="LC366" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L367" class="blob-num js-line-number" data-line-number="367"></td>
        <td id="LC367" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>TIME UPDATE</span></td>
      </tr>
      <tr>
        <td id="L368" class="blob-num js-line-number" data-line-number="368"></td>
        <td id="LC368" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>timeupdate<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L369" class="blob-num js-line-number" data-line-number="369"></td>
        <td id="LC369" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L370" class="blob-num js-line-number" data-line-number="370"></td>
        <td id="LC370" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span>							console.debug( &quot;2. timeupdate:: &quot;, data );</span></td>
      </tr>
      <tr>
        <td id="L371" class="blob-num js-line-number" data-line-number="371"></td>
        <td id="LC371" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L372" class="blob-num js-line-number" data-line-number="372"></td>
        <td id="LC372" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">duration</span> <span class="pl-k">=</span> <span class="pl-smi">data</span>.<span class="pl-smi">duration</span>;</td>
      </tr>
      <tr>
        <td id="L373" class="blob-num js-line-number" data-line-number="373"></td>
        <td id="LC373" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">percent</span> <span class="pl-k">=</span> <span class="pl-smi">data</span>.<span class="pl-smi">percent</span>;</td>
      </tr>
      <tr>
        <td id="L374" class="blob-num js-line-number" data-line-number="374"></td>
        <td id="LC374" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">seconds</span> <span class="pl-k">=</span> <span class="pl-smi">data</span>.<span class="pl-smi">seconds</span>;</td>
      </tr>
      <tr>
        <td id="L375" class="blob-num js-line-number" data-line-number="375"></td>
        <td id="LC375" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L376" class="blob-num js-line-number" data-line-number="376"></td>
        <td id="LC376" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">1</span>;</td>
      </tr>
      <tr>
        <td id="L377" class="blob-num js-line-number" data-line-number="377"></td>
        <td id="LC377" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">getPaused</span>().<span class="pl-en">then</span>( <span class="pl-k">function</span>( <span class="pl-smi">paused</span> ) {</td>
      </tr>
      <tr>
        <td id="L378" class="blob-num js-line-number" data-line-number="378"></td>
        <td id="LC378" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">if</span>( paused )</td>
      </tr>
      <tr>
        <td id="L379" class="blob-num js-line-number" data-line-number="379"></td>
        <td id="LC379" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">2</span>;</td>
      </tr>
      <tr>
        <td id="L380" class="blob-num js-line-number" data-line-number="380"></td>
        <td id="LC380" class="blob-code blob-code-inner js-file-line">							} );</td>
      </tr>
      <tr>
        <td id="L381" class="blob-num js-line-number" data-line-number="381"></td>
        <td id="LC381" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L382" class="blob-num js-line-number" data-line-number="382"></td>
        <td id="LC382" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopMovieOnBlur</span> ) {</td>
      </tr>
      <tr>
        <td id="L383" class="blob-num js-line-number" data-line-number="383"></td>
        <td id="LC383" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-c1">document</span>.<span class="pl-c1">hasFocus</span>() ) {</td>
      </tr>
      <tr>
        <td id="L384" class="blob-num js-line-number" data-line-number="384"></td>
        <td id="LC384" class="blob-code blob-code-inner js-file-line">									<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">==</span> <span class="pl-c1">1</span> ) {</td>
      </tr>
      <tr>
        <td id="L385" class="blob-num js-line-number" data-line-number="385"></td>
        <td id="LC385" class="blob-code blob-code-inner js-file-line">										<span class="pl-smi">vimeo_player</span>.<span class="pl-c1">hasFocus</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L386" class="blob-num js-line-number" data-line-number="386"></td>
        <td id="LC386" class="blob-code blob-code-inner js-file-line">										<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_pause</span>();</td>
      </tr>
      <tr>
        <td id="L387" class="blob-num js-line-number" data-line-number="387"></td>
        <td id="LC387" class="blob-code blob-code-inner js-file-line">										<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">document_focus</span> <span class="pl-k">=</span> <span class="pl-c1">setInterval</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L388" class="blob-num js-line-number" data-line-number="388"></td>
        <td id="LC388" class="blob-code blob-code-inner js-file-line">											<span class="pl-k">if</span>( <span class="pl-c1">document</span>.<span class="pl-c1">hasFocus</span>() <span class="pl-k">&amp;&amp;</span> <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-c1">hasFocus</span> ) {</td>
      </tr>
      <tr>
        <td id="L389" class="blob-num js-line-number" data-line-number="389"></td>
        <td id="LC389" class="blob-code blob-code-inner js-file-line">												<span class="pl-smi">vimeo_player</span>.<span class="pl-c1">hasFocus</span> <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L390" class="blob-num js-line-number" data-line-number="390"></td>
        <td id="LC390" class="blob-code blob-code-inner js-file-line">												<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_play</span>();</td>
      </tr>
      <tr>
        <td id="L391" class="blob-num js-line-number" data-line-number="391"></td>
        <td id="LC391" class="blob-code blob-code-inner js-file-line">												<span class="pl-c1">clearInterval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">document_focus</span> );</td>
      </tr>
      <tr>
        <td id="L392" class="blob-num js-line-number" data-line-number="392"></td>
        <td id="LC392" class="blob-code blob-code-inner js-file-line">											}</td>
      </tr>
      <tr>
        <td id="L393" class="blob-num js-line-number" data-line-number="393"></td>
        <td id="LC393" class="blob-code blob-code-inner js-file-line">										}, <span class="pl-c1">300</span> );</td>
      </tr>
      <tr>
        <td id="L394" class="blob-num js-line-number" data-line-number="394"></td>
        <td id="LC394" class="blob-code blob-code-inner js-file-line">									}</td>
      </tr>
      <tr>
        <td id="L395" class="blob-num js-line-number" data-line-number="395"></td>
        <td id="LC395" class="blob-code blob-code-inner js-file-line">								}</td>
      </tr>
      <tr>
        <td id="L396" class="blob-num js-line-number" data-line-number="396"></td>
        <td id="LC396" class="blob-code blob-code-inner js-file-line">							}</td>
      </tr>
      <tr>
        <td id="L397" class="blob-num js-line-number" data-line-number="397"></td>
        <td id="LC397" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L398" class="blob-num js-line-number" data-line-number="398"></td>
        <td id="LC398" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">showControls</span> ) {</td>
      </tr>
      <tr>
        <td id="L399" class="blob-num js-line-number" data-line-number="399"></td>
        <td id="LC399" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> controls <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> );</td>
      </tr>
      <tr>
        <td id="L400" class="blob-num js-line-number" data-line-number="400"></td>
        <td id="LC400" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> progressBar <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_pogress<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L401" class="blob-num js-line-number" data-line-number="401"></td>
        <td id="LC401" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> loadedBar <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_loaded<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L402" class="blob-num js-line-number" data-line-number="402"></td>
        <td id="LC402" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> timeBar <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_seek_bar<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L403" class="blob-num js-line-number" data-line-number="403"></td>
        <td id="LC403" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> totW <span class="pl-k">=</span> <span class="pl-smi">progressBar</span>.<span class="pl-c1">outerWidth</span>();</td>
      </tr>
      <tr>
        <td id="L404" class="blob-num js-line-number" data-line-number="404"></td>
        <td id="LC404" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> currentTime <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>( <span class="pl-smi">data</span>.<span class="pl-smi">seconds</span> );</td>
      </tr>
      <tr>
        <td id="L405" class="blob-num js-line-number" data-line-number="405"></td>
        <td id="LC405" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> totalTime <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>( <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> );</td>
      </tr>
      <tr>
        <td id="L406" class="blob-num js-line-number" data-line-number="406"></td>
        <td id="LC406" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> timeW <span class="pl-k">=</span> ( currentTime <span class="pl-k">*</span> totW ) <span class="pl-k">/</span> totalTime;</td>
      </tr>
      <tr>
        <td id="L407" class="blob-num js-line-number" data-line-number="407"></td>
        <td id="LC407" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> startLeft <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L408" class="blob-num js-line-number" data-line-number="408"></td>
        <td id="LC408" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> loadedW <span class="pl-k">=</span> <span class="pl-smi">data</span>.<span class="pl-smi">percent</span> <span class="pl-k">*</span> <span class="pl-c1">100</span>;</td>
      </tr>
      <tr>
        <td id="L409" class="blob-num js-line-number" data-line-number="409"></td>
        <td id="LC409" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">loadedBar</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L410" class="blob-num js-line-number" data-line-number="410"></td>
        <td id="LC410" class="blob-code blob-code-inner js-file-line">									left<span class="pl-k">:</span> startLeft,</td>
      </tr>
      <tr>
        <td id="L411" class="blob-num js-line-number" data-line-number="411"></td>
        <td id="LC411" class="blob-code blob-code-inner js-file-line">									width<span class="pl-k">:</span> loadedW <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span>%<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L412" class="blob-num js-line-number" data-line-number="412"></td>
        <td id="LC412" class="blob-code blob-code-inner js-file-line">								} );</td>
      </tr>
      <tr>
        <td id="L413" class="blob-num js-line-number" data-line-number="413"></td>
        <td id="LC413" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">timeBar</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L414" class="blob-num js-line-number" data-line-number="414"></td>
        <td id="LC414" class="blob-code blob-code-inner js-file-line">									left<span class="pl-k">:</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L415" class="blob-num js-line-number" data-line-number="415"></td>
        <td id="LC415" class="blob-code blob-code-inner js-file-line">									width<span class="pl-k">:</span> timeW</td>
      </tr>
      <tr>
        <td id="L416" class="blob-num js-line-number" data-line-number="416"></td>
        <td id="LC416" class="blob-code blob-code-inner js-file-line">								} );</td>
      </tr>
      <tr>
        <td id="L417" class="blob-num js-line-number" data-line-number="417"></td>
        <td id="LC417" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L418" class="blob-num js-line-number" data-line-number="418"></td>
        <td id="LC418" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">if</span>( <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> ) {</td>
      </tr>
      <tr>
        <td id="L419" class="blob-num js-line-number" data-line-number="419"></td>
        <td id="LC419" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_time<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-en">formatTime</span>( <span class="pl-smi">data</span>.<span class="pl-smi">seconds</span> ) <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span> / <span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-en">formatTime</span>( <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> ) );</td>
      </tr>
      <tr>
        <td id="L420" class="blob-num js-line-number" data-line-number="420"></td>
        <td id="LC420" class="blob-code blob-code-inner js-file-line">								} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L421" class="blob-num js-line-number" data-line-number="421"></td>
        <td id="LC421" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_time<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>-- : -- / -- : --<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L422" class="blob-num js-line-number" data-line-number="422"></td>
        <td id="LC422" class="blob-code blob-code-inner js-file-line">								}</td>
      </tr>
      <tr>
        <td id="L423" class="blob-num js-line-number" data-line-number="423"></td>
        <td id="LC423" class="blob-code blob-code-inner js-file-line">							}</td>
      </tr>
      <tr>
        <td id="L424" class="blob-num js-line-number" data-line-number="424"></td>
        <td id="LC424" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L425" class="blob-num js-line-number" data-line-number="425"></td>
        <td id="LC425" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">addRaster</span> ) {</td>
      </tr>
      <tr>
        <td id="L426" class="blob-num js-line-number" data-line-number="426"></td>
        <td id="LC426" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">var</span> classN <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">addRaster</span> <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>dot<span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>raster-dot<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>raster<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L427" class="blob-num js-line-number" data-line-number="427"></td>
        <td id="LC427" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span>.<span class="pl-en">addClass</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isRetina</span> <span class="pl-k">?</span> classN <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span> retina<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> classN );</td>
      </tr>
      <tr>
        <td id="L428" class="blob-num js-line-number" data-line-number="428"></td>
        <td id="LC428" class="blob-code blob-code-inner js-file-line">							} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L429" class="blob-num js-line-number" data-line-number="429"></td>
        <td id="LC429" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span>.<span class="pl-en">removeClass</span>( <span class="pl-k">function</span>( <span class="pl-smi">index</span>, <span class="pl-smi">classNames</span> ) {</td>
      </tr>
      <tr>
        <td id="L430" class="blob-num js-line-number" data-line-number="430"></td>
        <td id="LC430" class="blob-code blob-code-inner js-file-line">									<span class="pl-c"><span class="pl-c">//</span> change the list into an array</span></td>
      </tr>
      <tr>
        <td id="L431" class="blob-num js-line-number" data-line-number="431"></td>
        <td id="LC431" class="blob-code blob-code-inner js-file-line">									<span class="pl-k">var</span> current_classes <span class="pl-k">=</span> <span class="pl-smi">classNames</span>.<span class="pl-c1">split</span>( <span class="pl-s"><span class="pl-pds">&quot;</span> <span class="pl-pds">&quot;</span></span> ),</td>
      </tr>
      <tr>
        <td id="L432" class="blob-num js-line-number" data-line-number="432"></td>
        <td id="LC432" class="blob-code blob-code-inner js-file-line">										<span class="pl-c"><span class="pl-c">//</span> array of classes which are to be removed</span></td>
      </tr>
      <tr>
        <td id="L433" class="blob-num js-line-number" data-line-number="433"></td>
        <td id="LC433" class="blob-code blob-code-inner js-file-line">										classes_to_remove <span class="pl-k">=</span> [];</td>
      </tr>
      <tr>
        <td id="L434" class="blob-num js-line-number" data-line-number="434"></td>
        <td id="LC434" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">jQuery</span>.<span class="pl-en">each</span>( current_classes, <span class="pl-k">function</span>( <span class="pl-smi">index</span>, <span class="pl-smi">class_name</span> ) {</td>
      </tr>
      <tr>
        <td id="L435" class="blob-num js-line-number" data-line-number="435"></td>
        <td id="LC435" class="blob-code blob-code-inner js-file-line">										<span class="pl-c"><span class="pl-c">//</span> if the classname begins with bg add it to the classes_to_remove array</span></td>
      </tr>
      <tr>
        <td id="L436" class="blob-num js-line-number" data-line-number="436"></td>
        <td id="LC436" class="blob-code blob-code-inner js-file-line">										<span class="pl-k">if</span>(<span class="pl-sr"> <span class="pl-pds">/</span>raster<span class="pl-c1">.</span><span class="pl-k">*</span><span class="pl-pds">/</span></span>.<span class="pl-c1">test</span>( class_name ) ) {</td>
      </tr>
      <tr>
        <td id="L437" class="blob-num js-line-number" data-line-number="437"></td>
        <td id="LC437" class="blob-code blob-code-inner js-file-line">											<span class="pl-smi">classes_to_remove</span>.<span class="pl-c1">push</span>( class_name );</td>
      </tr>
      <tr>
        <td id="L438" class="blob-num js-line-number" data-line-number="438"></td>
        <td id="LC438" class="blob-code blob-code-inner js-file-line">										}</td>
      </tr>
      <tr>
        <td id="L439" class="blob-num js-line-number" data-line-number="439"></td>
        <td id="LC439" class="blob-code blob-code-inner js-file-line">									} );</td>
      </tr>
      <tr>
        <td id="L440" class="blob-num js-line-number" data-line-number="440"></td>
        <td id="LC440" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">classes_to_remove</span>.<span class="pl-c1">push</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>retina<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L441" class="blob-num js-line-number" data-line-number="441"></td>
        <td id="LC441" class="blob-code blob-code-inner js-file-line">									<span class="pl-c"><span class="pl-c">//</span> turn the array back into a string</span></td>
      </tr>
      <tr>
        <td id="L442" class="blob-num js-line-number" data-line-number="442"></td>
        <td id="LC442" class="blob-code blob-code-inner js-file-line">									<span class="pl-k">return</span> <span class="pl-smi">classes_to_remove</span>.<span class="pl-c1">join</span>( <span class="pl-s"><span class="pl-pds">&quot;</span> <span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L443" class="blob-num js-line-number" data-line-number="443"></td>
        <td id="LC443" class="blob-code blob-code-inner js-file-line">								} )</td>
      </tr>
      <tr>
        <td id="L444" class="blob-num js-line-number" data-line-number="444"></td>
        <td id="LC444" class="blob-code blob-code-inner js-file-line">							}</td>
      </tr>
      <tr>
        <td id="L445" class="blob-num js-line-number" data-line-number="445"></td>
        <td id="LC445" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L446" class="blob-num js-line-number" data-line-number="446"></td>
        <td id="LC446" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> <span class="pl-k">&gt;</span> <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> <span class="pl-k">?</span> <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> <span class="pl-k">-</span> <span class="pl-c1">0.5</span> <span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span>;</td>
      </tr>
      <tr>
        <td id="L447" class="blob-num js-line-number" data-line-number="447"></td>
        <td id="LC447" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">var</span> end_time <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> <span class="pl-k">||</span> <span class="pl-smi">data</span>.<span class="pl-smi">duration</span> <span class="pl-k">-</span> <span class="pl-c1">0.5</span>;</td>
      </tr>
      <tr>
        <td id="L448" class="blob-num js-line-number" data-line-number="448"></td>
        <td id="LC448" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L449" class="blob-num js-line-number" data-line-number="449"></td>
        <td id="LC449" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">if</span>( <span class="pl-smi">data</span>.<span class="pl-smi">seconds</span> <span class="pl-k">&gt;=</span> end_time ) {</td>
      </tr>
      <tr>
        <td id="L450" class="blob-num js-line-number" data-line-number="450"></td>
        <td id="LC450" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L451" class="blob-num js-line-number" data-line-number="451"></td>
        <td id="LC451" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">loop</span> <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">loop</span> <span class="pl-k">||</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L452" class="blob-num js-line-number" data-line-number="452"></td>
        <td id="LC452" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L453" class="blob-num js-line-number" data-line-number="453"></td>
        <td id="LC453" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">loop</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">loop</span> <span class="pl-k">&lt;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">loop</span> ) {</td>
      </tr>
      <tr>
        <td id="L454" class="blob-num js-line-number" data-line-number="454"></td>
        <td id="LC454" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_seekTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">startAt</span> );</td>
      </tr>
      <tr>
        <td id="L455" class="blob-num js-line-number" data-line-number="455"></td>
        <td id="LC455" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">loop</span><span class="pl-k">++</span>;</td>
      </tr>
      <tr>
        <td id="L456" class="blob-num js-line-number" data-line-number="456"></td>
        <td id="LC456" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L457" class="blob-num js-line-number" data-line-number="457"></td>
        <td id="LC457" class="blob-code blob-code-inner js-file-line">								} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L458" class="blob-num js-line-number" data-line-number="458"></td>
        <td id="LC458" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_pause</span>();</td>
      </tr>
      <tr>
        <td id="L459" class="blob-num js-line-number" data-line-number="459"></td>
        <td id="LC459" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L460" class="blob-num js-line-number" data-line-number="460"></td>
        <td id="LC460" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L461" class="blob-num js-line-number" data-line-number="461"></td>
        <td id="LC461" class="blob-code blob-code-inner js-file-line">								}</td>
      </tr>
      <tr>
        <td id="L462" class="blob-num js-line-number" data-line-number="462"></td>
        <td id="LC462" class="blob-code blob-code-inner js-file-line">							}</td>
      </tr>
      <tr>
        <td id="L463" class="blob-num js-line-number" data-line-number="463"></td>
        <td id="LC463" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L464" class="blob-num js-line-number" data-line-number="464"></td>
        <td id="LC464" class="blob-code blob-code-inner js-file-line">							<span class="pl-c"><span class="pl-c">//</span> Trigger state events</span></td>
      </tr>
      <tr>
        <td id="L465" class="blob-num js-line-number" data-line-number="465"></td>
        <td id="LC465" class="blob-code blob-code-inner js-file-line">							VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPTime<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L466" class="blob-num js-line-number" data-line-number="466"></td>
        <td id="LC466" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">VEvent</span>.<span class="pl-smi">time</span> <span class="pl-k">=</span> <span class="pl-smi">data</span>.<span class="pl-smi">seconds</span>;</td>
      </tr>
      <tr>
        <td id="L467" class="blob-num js-line-number" data-line-number="467"></td>
        <td id="LC467" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L468" class="blob-num js-line-number" data-line-number="468"></td>
        <td id="LC468" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L469" class="blob-num js-line-number" data-line-number="469"></td>
        <td id="LC469" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L470" class="blob-num js-line-number" data-line-number="470"></td>
        <td id="LC470" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L471" class="blob-num js-line-number" data-line-number="471"></td>
        <td id="LC471" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L472" class="blob-num js-line-number" data-line-number="472"></td>
        <td id="LC472" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>change_state<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L473" class="blob-num js-line-number" data-line-number="473"></td>
        <td id="LC473" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span>console.debug( &quot;player state:: &quot;, vimeo_player.state );</span></td>
      </tr>
      <tr>
        <td id="L474" class="blob-num js-line-number" data-line-number="474"></td>
        <td id="LC474" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L475" class="blob-num js-line-number" data-line-number="475"></td>
        <td id="LC475" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">==</span> <span class="pl-c1">0</span> )</td>
      </tr>
      <tr>
        <td id="L476" class="blob-num js-line-number" data-line-number="476"></td>
        <td id="LC476" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>.<span class="pl-en">fadeOut</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L477" class="blob-num js-line-number" data-line-number="477"></td>
        <td id="LC477" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_seekTo</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L478" class="blob-num js-line-number" data-line-number="478"></td>
        <td id="LC478" class="blob-code blob-code-inner js-file-line">							} );</td>
      </tr>
      <tr>
        <td id="L479" class="blob-num js-line-number" data-line-number="479"></td>
        <td id="LC479" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L480" class="blob-num js-line-number" data-line-number="480"></td>
        <td id="LC480" class="blob-code blob-code-inner js-file-line">					} )</td>
      </tr>
      <tr>
        <td id="L481" class="blob-num js-line-number" data-line-number="481"></td>
        <td id="LC481" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L482" class="blob-num js-line-number" data-line-number="482"></td>
        <td id="LC482" class="blob-code blob-code-inner js-file-line">			} )</td>
      </tr>
      <tr>
        <td id="L483" class="blob-num js-line-number" data-line-number="483"></td>
        <td id="LC483" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L484" class="blob-num js-line-number" data-line-number="484"></td>
        <td id="LC484" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L485" class="blob-num js-line-number" data-line-number="485"></td>
        <td id="LC485" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">formatTime</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">s</span> ) {</td>
      </tr>
      <tr>
        <td id="L486" class="blob-num js-line-number" data-line-number="486"></td>
        <td id="LC486" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> min <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>( s <span class="pl-k">/</span> <span class="pl-c1">60</span> );</td>
      </tr>
      <tr>
        <td id="L487" class="blob-num js-line-number" data-line-number="487"></td>
        <td id="LC487" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> sec <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>( s <span class="pl-k">-</span> ( <span class="pl-c1">60</span> <span class="pl-k">*</span> min ) );</td>
      </tr>
      <tr>
        <td id="L488" class="blob-num js-line-number" data-line-number="488"></td>
        <td id="LC488" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span>( min <span class="pl-k">&lt;=</span> <span class="pl-c1">9</span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>0<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> min <span class="pl-k">:</span> min ) <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span> : <span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> ( sec <span class="pl-k">&lt;=</span> <span class="pl-c1">9</span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>0<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> sec <span class="pl-k">:</span> sec );</td>
      </tr>
      <tr>
        <td id="L489" class="blob-num js-line-number" data-line-number="489"></td>
        <td id="LC489" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L490" class="blob-num js-line-number" data-line-number="490"></td>
        <td id="LC490" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L491" class="blob-num js-line-number" data-line-number="491"></td>
        <td id="LC491" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">play</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L492" class="blob-num js-line-number" data-line-number="492"></td>
        <td id="LC492" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L493" class="blob-num js-line-number" data-line-number="493"></td>
        <td id="LC493" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isReady</span> )</td>
      </tr>
      <tr>
        <td id="L494" class="blob-num js-line-number" data-line-number="494"></td>
        <td id="LC494" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L495" class="blob-num js-line-number" data-line-number="495"></td>
        <td id="LC495" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L496" class="blob-num js-line-number" data-line-number="496"></td>
        <td id="LC496" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-c1">play</span>();</td>
      </tr>
      <tr>
        <td id="L497" class="blob-num js-line-number" data-line-number="497"></td>
        <td id="LC497" class="blob-code blob-code-inner js-file-line">			<span class="pl-c1">setTimeout</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L498" class="blob-num js-line-number" data-line-number="498"></td>
        <td id="LC498" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>.<span class="pl-en">fadeTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">opacity</span> );</td>
      </tr>
      <tr>
        <td id="L499" class="blob-num js-line-number" data-line-number="499"></td>
        <td id="LC499" class="blob-code blob-code-inner js-file-line">			}, <span class="pl-c1">1000</span> );</td>
      </tr>
      <tr>
        <td id="L500" class="blob-num js-line-number" data-line-number="500"></td>
        <td id="LC500" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L501" class="blob-num js-line-number" data-line-number="501"></td>
        <td id="LC501" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> controls <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> );</td>
      </tr>
      <tr>
        <td id="L502" class="blob-num js-line-number" data-line-number="502"></td>
        <td id="LC502" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L503" class="blob-num js-line-number" data-line-number="503"></td>
        <td id="LC503" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">controls</span>.<span class="pl-c1">length</span> ) {</td>
      </tr>
      <tr>
        <td id="L504" class="blob-num js-line-number" data-line-number="504"></td>
        <td id="LC504" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> playBtn <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.mb_YTPPvimeo_player_playpause<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L505" class="blob-num js-line-number" data-line-number="505"></td>
        <td id="LC505" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">playBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">pause</span> );</td>
      </tr>
      <tr>
        <td id="L506" class="blob-num js-line-number" data-line-number="506"></td>
        <td id="LC506" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L507" class="blob-num js-line-number" data-line-number="507"></td>
        <td id="LC507" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">1</span>;</td>
      </tr>
      <tr>
        <td id="L508" class="blob-num js-line-number" data-line-number="508"></td>
        <td id="LC508" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L509" class="blob-num js-line-number" data-line-number="509"></td>
        <td id="LC509" class="blob-code blob-code-inner js-file-line">			<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>background-image<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>none<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L510" class="blob-num js-line-number" data-line-number="510"></td>
        <td id="LC510" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L511" class="blob-num js-line-number" data-line-number="511"></td>
        <td id="LC511" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L512" class="blob-num js-line-number" data-line-number="512"></td>
        <td id="LC512" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L513" class="blob-num js-line-number" data-line-number="513"></td>
        <td id="LC513" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">togglePlay</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">callback</span> ) {</td>
      </tr>
      <tr>
        <td id="L514" class="blob-num js-line-number" data-line-number="514"></td>
        <td id="LC514" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L515" class="blob-num js-line-number" data-line-number="515"></td>
        <td id="LC515" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">==</span> <span class="pl-c1">1</span> )</td>
      </tr>
      <tr>
        <td id="L516" class="blob-num js-line-number" data-line-number="516"></td>
        <td id="LC516" class="blob-code blob-code-inner js-file-line">				<span class="pl-c1">this</span>.<span class="pl-en">v_pause</span>();</td>
      </tr>
      <tr>
        <td id="L517" class="blob-num js-line-number" data-line-number="517"></td>
        <td id="LC517" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L518" class="blob-num js-line-number" data-line-number="518"></td>
        <td id="LC518" class="blob-code blob-code-inner js-file-line">				<span class="pl-c1">this</span>.<span class="pl-en">v_play</span>();</td>
      </tr>
      <tr>
        <td id="L519" class="blob-num js-line-number" data-line-number="519"></td>
        <td id="LC519" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L520" class="blob-num js-line-number" data-line-number="520"></td>
        <td id="LC520" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">typeof</span> callback <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>function<span class="pl-pds">&quot;</span></span> )</td>
      </tr>
      <tr>
        <td id="L521" class="blob-num js-line-number" data-line-number="521"></td>
        <td id="LC521" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">callback</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> );</td>
      </tr>
      <tr>
        <td id="L522" class="blob-num js-line-number" data-line-number="522"></td>
        <td id="LC522" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L523" class="blob-num js-line-number" data-line-number="523"></td>
        <td id="LC523" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L524" class="blob-num js-line-number" data-line-number="524"></td>
        <td id="LC524" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L525" class="blob-num js-line-number" data-line-number="525"></td>
        <td id="LC525" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L526" class="blob-num js-line-number" data-line-number="526"></td>
        <td id="LC526" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">pause</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L527" class="blob-num js-line-number" data-line-number="527"></td>
        <td id="LC527" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L528" class="blob-num js-line-number" data-line-number="528"></td>
        <td id="LC528" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-c1">pause</span>();</td>
      </tr>
      <tr>
        <td id="L529" class="blob-num js-line-number" data-line-number="529"></td>
        <td id="LC529" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">=</span> <span class="pl-c1">2</span>;</td>
      </tr>
      <tr>
        <td id="L530" class="blob-num js-line-number" data-line-number="530"></td>
        <td id="LC530" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L531" class="blob-num js-line-number" data-line-number="531"></td>
        <td id="LC531" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L532" class="blob-num js-line-number" data-line-number="532"></td>
        <td id="LC532" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L533" class="blob-num js-line-number" data-line-number="533"></td>
        <td id="LC533" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">seekTo</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">val</span>, <span class="pl-smi">callback</span> ) {</td>
      </tr>
      <tr>
        <td id="L534" class="blob-num js-line-number" data-line-number="534"></td>
        <td id="LC534" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L535" class="blob-num js-line-number" data-line-number="535"></td>
        <td id="LC535" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L536" class="blob-num js-line-number" data-line-number="536"></td>
        <td id="LC536" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> seekTo <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> <span class="pl-k">&amp;&amp;</span> ( val <span class="pl-k">&gt;=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> ) <span class="pl-k">?</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">stopAt</span> <span class="pl-k">-</span> <span class="pl-c1">0.5</span> <span class="pl-k">:</span> val;</td>
      </tr>
      <tr>
        <td id="L537" class="blob-num js-line-number" data-line-number="537"></td>
        <td id="LC537" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L538" class="blob-num js-line-number" data-line-number="538"></td>
        <td id="LC538" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">setCurrentTime</span>( seekTo ).<span class="pl-en">then</span>( <span class="pl-k">function</span>( <span class="pl-smi">data</span> ) {</td>
      </tr>
      <tr>
        <td id="L539" class="blob-num js-line-number" data-line-number="539"></td>
        <td id="LC539" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">typeof</span> callback <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>function<span class="pl-pds">&quot;</span></span> )</td>
      </tr>
      <tr>
        <td id="L540" class="blob-num js-line-number" data-line-number="540"></td>
        <td id="LC540" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">callback</span>( data );</td>
      </tr>
      <tr>
        <td id="L541" class="blob-num js-line-number" data-line-number="541"></td>
        <td id="LC541" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L542" class="blob-num js-line-number" data-line-number="542"></td>
        <td id="LC542" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L543" class="blob-num js-line-number" data-line-number="543"></td>
        <td id="LC543" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L544" class="blob-num js-line-number" data-line-number="544"></td>
        <td id="LC544" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L545" class="blob-num js-line-number" data-line-number="545"></td>
        <td id="LC545" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">setVolume</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">val</span> ) {</td>
      </tr>
      <tr>
        <td id="L546" class="blob-num js-line-number" data-line-number="546"></td>
        <td id="LC546" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L547" class="blob-num js-line-number" data-line-number="547"></td>
        <td id="LC547" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L548" class="blob-num js-line-number" data-line-number="548"></td>
        <td id="LC548" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L549" class="blob-num js-line-number" data-line-number="549"></td>
        <td id="LC549" class="blob-code blob-code-inner js-file-line">			<span class="pl-c">/*</span></td>
      </tr>
      <tr>
        <td id="L550" class="blob-num js-line-number" data-line-number="550"></td>
        <td id="LC550" class="blob-code blob-code-inner js-file-line"><span class="pl-c">						console.debug( &quot;setVolume:: &quot;, val );</span></td>
      </tr>
      <tr>
        <td id="L551" class="blob-num js-line-number" data-line-number="551"></td>
        <td id="LC551" class="blob-code blob-code-inner js-file-line"><span class="pl-c">						console.debug( &quot;volume:: &quot;, vimeo_player.opt.vol );</span></td>
      </tr>
      <tr>
        <td id="L552" class="blob-num js-line-number" data-line-number="552"></td>
        <td id="LC552" class="blob-code blob-code-inner js-file-line"><span class="pl-c">			*/</span></td>
      </tr>
      <tr>
        <td id="L553" class="blob-num js-line-number" data-line-number="553"></td>
        <td id="LC553" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L554" class="blob-num js-line-number" data-line-number="554"></td>
        <td id="LC554" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span>val <span class="pl-k">&amp;&amp;</span> <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> )</td>
      </tr>
      <tr>
        <td id="L555" class="blob-num js-line-number" data-line-number="555"></td>
        <td id="LC555" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_unmute</span>();</td>
      </tr>
      <tr>
        <td id="L556" class="blob-num js-line-number" data-line-number="556"></td>
        <td id="LC556" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">else</span> <span class="pl-k">if</span>( ( <span class="pl-k">!</span>val <span class="pl-k">&amp;&amp;</span> <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> ) <span class="pl-k">||</span> ( val <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">==</span> val ) ) {</td>
      </tr>
      <tr>
        <td id="L557" class="blob-num js-line-number" data-line-number="557"></td>
        <td id="LC557" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> )</td>
      </tr>
      <tr>
        <td id="L558" class="blob-num js-line-number" data-line-number="558"></td>
        <td id="LC558" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_unmute</span>();</td>
      </tr>
      <tr>
        <td id="L559" class="blob-num js-line-number" data-line-number="559"></td>
        <td id="LC559" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L560" class="blob-num js-line-number" data-line-number="560"></td>
        <td id="LC560" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_mute</span>();</td>
      </tr>
      <tr>
        <td id="L561" class="blob-num js-line-number" data-line-number="561"></td>
        <td id="LC561" class="blob-code blob-code-inner js-file-line">			} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L562" class="blob-num js-line-number" data-line-number="562"></td>
        <td id="LC562" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L563" class="blob-num js-line-number" data-line-number="563"></td>
        <td id="LC563" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">=</span> val;</td>
      </tr>
      <tr>
        <td id="L564" class="blob-num js-line-number" data-line-number="564"></td>
        <td id="LC564" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">setVolume</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> );</td>
      </tr>
      <tr>
        <td id="L565" class="blob-num js-line-number" data-line-number="565"></td>
        <td id="LC565" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L566" class="blob-num js-line-number" data-line-number="566"></td>
        <td id="LC566" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-en">updateSliderVal</span>( val <span class="pl-k">*</span> <span class="pl-c1">100</span> )</td>
      </tr>
      <tr>
        <td id="L567" class="blob-num js-line-number" data-line-number="567"></td>
        <td id="LC567" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L568" class="blob-num js-line-number" data-line-number="568"></td>
        <td id="LC568" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L569" class="blob-num js-line-number" data-line-number="569"></td>
        <td id="LC569" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L570" class="blob-num js-line-number" data-line-number="570"></td>
        <td id="LC570" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L571" class="blob-num js-line-number" data-line-number="571"></td>
        <td id="LC571" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">toggleVolume</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L572" class="blob-num js-line-number" data-line-number="572"></td>
        <td id="LC572" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L573" class="blob-num js-line-number" data-line-number="573"></td>
        <td id="LC573" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span>vimeo_player ) <span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L574" class="blob-num js-line-number" data-line-number="574"></td>
        <td id="LC574" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L575" class="blob-num js-line-number" data-line-number="575"></td>
        <td id="LC575" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> ) {</td>
      </tr>
      <tr>
        <td id="L576" class="blob-num js-line-number" data-line-number="576"></td>
        <td id="LC576" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_unmute</span>();</td>
      </tr>
      <tr>
        <td id="L577" class="blob-num js-line-number" data-line-number="577"></td>
        <td id="LC577" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L578" class="blob-num js-line-number" data-line-number="578"></td>
        <td id="LC578" class="blob-code blob-code-inner js-file-line">			} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L579" class="blob-num js-line-number" data-line-number="579"></td>
        <td id="LC579" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_mute</span>();</td>
      </tr>
      <tr>
        <td id="L580" class="blob-num js-line-number" data-line-number="580"></td>
        <td id="LC580" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L581" class="blob-num js-line-number" data-line-number="581"></td>
        <td id="LC581" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L582" class="blob-num js-line-number" data-line-number="582"></td>
        <td id="LC582" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L583" class="blob-num js-line-number" data-line-number="583"></td>
        <td id="LC583" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L584" class="blob-num js-line-number" data-line-number="584"></td>
        <td id="LC584" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">mute</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L585" class="blob-num js-line-number" data-line-number="585"></td>
        <td id="LC585" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L586" class="blob-num js-line-number" data-line-number="586"></td>
        <td id="LC586" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> )</td>
      </tr>
      <tr>
        <td id="L587" class="blob-num js-line-number" data-line-number="587"></td>
        <td id="LC587" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L588" class="blob-num js-line-number" data-line-number="588"></td>
        <td id="LC588" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L589" class="blob-num js-line-number" data-line-number="589"></td>
        <td id="LC589" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">setVolume</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L590" class="blob-num js-line-number" data-line-number="590"></td>
        <td id="LC590" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">length</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">width</span>() <span class="pl-k">&gt;</span> <span class="pl-c1">10</span> ) {</td>
      </tr>
      <tr>
        <td id="L591" class="blob-num js-line-number" data-line-number="591"></td>
        <td id="LC591" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-en">updateSliderVal</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L592" class="blob-num js-line-number" data-line-number="592"></td>
        <td id="LC592" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L593" class="blob-num js-line-number" data-line-number="593"></td>
        <td id="LC593" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> controls <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> );</td>
      </tr>
      <tr>
        <td id="L594" class="blob-num js-line-number" data-line-number="594"></td>
        <td id="LC594" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> muteBtn <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_muteUnmute<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L595" class="blob-num js-line-number" data-line-number="595"></td>
        <td id="LC595" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">muteBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">unmute</span> );</td>
      </tr>
      <tr>
        <td id="L596" class="blob-num js-line-number" data-line-number="596"></td>
        <td id="LC596" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L597" class="blob-num js-line-number" data-line-number="597"></td>
        <td id="LC597" class="blob-code blob-code-inner js-file-line">			<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>isMuted<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L598" class="blob-num js-line-number" data-line-number="598"></td>
        <td id="LC598" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L599" class="blob-num js-line-number" data-line-number="599"></td>
        <td id="LC599" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L600" class="blob-num js-line-number" data-line-number="600"></td>
        <td id="LC600" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>muted<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L601" class="blob-num js-line-number" data-line-number="601"></td>
        <td id="LC601" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L602" class="blob-num js-line-number" data-line-number="602"></td>
        <td id="LC602" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L603" class="blob-num js-line-number" data-line-number="603"></td>
        <td id="LC603" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L604" class="blob-num js-line-number" data-line-number="604"></td>
        <td id="LC604" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L605" class="blob-num js-line-number" data-line-number="605"></td>
        <td id="LC605" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">unmute</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L606" class="blob-num js-line-number" data-line-number="606"></td>
        <td id="LC606" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L607" class="blob-num js-line-number" data-line-number="607"></td>
        <td id="LC607" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L608" class="blob-num js-line-number" data-line-number="608"></td>
        <td id="LC608" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> )</td>
      </tr>
      <tr>
        <td id="L609" class="blob-num js-line-number" data-line-number="609"></td>
        <td id="LC609" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L610" class="blob-num js-line-number" data-line-number="610"></td>
        <td id="LC610" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L611" class="blob-num js-line-number" data-line-number="611"></td>
        <td id="LC611" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L612" class="blob-num js-line-number" data-line-number="612"></td>
        <td id="LC612" class="blob-code blob-code-inner js-file-line">			<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_set_volume</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> );</td>
      </tr>
      <tr>
        <td id="L613" class="blob-num js-line-number" data-line-number="613"></td>
        <td id="LC613" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L614" class="blob-num js-line-number" data-line-number="614"></td>
        <td id="LC614" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">length</span> ) <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-en">updateSliderVal</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">&gt;</span> <span class="pl-c1">.1</span> <span class="pl-k">?</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">:</span> <span class="pl-c1">.1</span> );</td>
      </tr>
      <tr>
        <td id="L615" class="blob-num js-line-number" data-line-number="615"></td>
        <td id="LC615" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> controls <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> );</td>
      </tr>
      <tr>
        <td id="L616" class="blob-num js-line-number" data-line-number="616"></td>
        <td id="LC616" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> muteBtn <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_muteUnmute<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L617" class="blob-num js-line-number" data-line-number="617"></td>
        <td id="LC617" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">muteBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">mute</span> );</td>
      </tr>
      <tr>
        <td id="L618" class="blob-num js-line-number" data-line-number="618"></td>
        <td id="LC618" class="blob-code blob-code-inner js-file-line">			<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">removeClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>isMuted<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L619" class="blob-num js-line-number" data-line-number="619"></td>
        <td id="LC619" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L620" class="blob-num js-line-number" data-line-number="620"></td>
        <td id="LC620" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span>.<span class="pl-en">removeClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>muted<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L621" class="blob-num js-line-number" data-line-number="621"></td>
        <td id="LC621" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L622" class="blob-num js-line-number" data-line-number="622"></td>
        <td id="LC622" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L623" class="blob-num js-line-number" data-line-number="623"></td>
        <td id="LC623" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L624" class="blob-num js-line-number" data-line-number="624"></td>
        <td id="LC624" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L625" class="blob-num js-line-number" data-line-number="625"></td>
        <td id="LC625" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">changeMovie</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">obj</span> ) {</td>
      </tr>
      <tr>
        <td id="L626" class="blob-num js-line-number" data-line-number="626"></td>
        <td id="LC626" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L627" class="blob-num js-line-number" data-line-number="627"></td>
        <td id="LC627" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L628" class="blob-num js-line-number" data-line-number="628"></td>
        <td id="LC628" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">loadVideo</span>( <span class="pl-smi">obj</span>.<span class="pl-smi">url</span> ).<span class="pl-en">then</span>( <span class="pl-k">function</span>( <span class="pl-smi">id</span> ) {</td>
      </tr>
      <tr>
        <td id="L629" class="blob-num js-line-number" data-line-number="629"></td>
        <td id="LC629" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L630" class="blob-num js-line-number" data-line-number="630"></td>
        <td id="LC630" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_setState</span>();</td>
      </tr>
      <tr>
        <td id="L631" class="blob-num js-line-number" data-line-number="631"></td>
        <td id="LC631" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L632" class="blob-num js-line-number" data-line-number="632"></td>
        <td id="LC632" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L633" class="blob-num js-line-number" data-line-number="633"></td>
        <td id="LC633" class="blob-code blob-code-inner js-file-line">			} )</td>
      </tr>
      <tr>
        <td id="L634" class="blob-num js-line-number" data-line-number="634"></td>
        <td id="LC634" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L635" class="blob-num js-line-number" data-line-number="635"></td>
        <td id="LC635" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L636" class="blob-num js-line-number" data-line-number="636"></td>
        <td id="LC636" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L637" class="blob-num js-line-number" data-line-number="637"></td>
        <td id="LC637" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">buildControls</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">vimeo_player</span> ) {</td>
      </tr>
      <tr>
        <td id="L638" class="blob-num js-line-number" data-line-number="638"></td>
        <td id="LC638" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> data <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>;</td>
      </tr>
      <tr>
        <td id="L639" class="blob-num js-line-number" data-line-number="639"></td>
        <td id="LC639" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L640" class="blob-num js-line-number" data-line-number="640"></td>
        <td id="LC640" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> ).<span class="pl-c1">length</span> )</td>
      </tr>
      <tr>
        <td id="L641" class="blob-num js-line-number" data-line-number="641"></td>
        <td id="LC641" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">return</span>;</td>
      </tr>
      <tr>
        <td id="L642" class="blob-num js-line-number" data-line-number="642"></td>
        <td id="LC642" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L643" class="blob-num js-line-number" data-line-number="643"></td>
        <td id="LC643" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>id<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_bar<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L644" class="blob-num js-line-number" data-line-number="644"></td>
        <td id="LC644" class="blob-code blob-code-inner js-file-line">				whiteSpace<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>noWrap<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L645" class="blob-num js-line-number" data-line-number="645"></td>
        <td id="LC645" class="blob-code blob-code-inner js-file-line">				position<span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>fixed<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L646" class="blob-num js-line-number" data-line-number="646"></td>
        <td id="LC646" class="blob-code blob-code-inner js-file-line">				zIndex<span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> <span class="pl-k">?</span> <span class="pl-c1">10000</span> <span class="pl-k">:</span> <span class="pl-c1">1000</span></td>
      </tr>
      <tr>
        <td id="L647" class="blob-num js-line-number" data-line-number="647"></td>
        <td id="LC647" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L648" class="blob-num js-line-number" data-line-number="648"></td>
        <td id="LC648" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> buttonBar <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>buttonBar<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L649" class="blob-num js-line-number" data-line-number="649"></td>
        <td id="LC649" class="blob-code blob-code-inner js-file-line">			<span class="pl-c">/* play/pause button*/</span></td>
      </tr>
      <tr>
        <td id="L650" class="blob-num js-line-number" data-line-number="650"></td>
        <td id="LC650" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> playpause <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span&gt;<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">play</span> <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;/span&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_pause vimeo_icon<span class="pl-pds">&quot;</span></span> ).<span class="pl-c1">click</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L651" class="blob-num js-line-number" data-line-number="651"></td>
        <td id="LC651" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">state</span> <span class="pl-k">==</span> <span class="pl-c1">1</span> ) <span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_pause</span>();</td>
      </tr>
      <tr>
        <td id="L652" class="blob-num js-line-number" data-line-number="652"></td>
        <td id="LC652" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">else</span> <span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_play</span>();</td>
      </tr>
      <tr>
        <td id="L653" class="blob-num js-line-number" data-line-number="653"></td>
        <td id="LC653" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L654" class="blob-num js-line-number" data-line-number="654"></td>
        <td id="LC654" class="blob-code blob-code-inner js-file-line">			<span class="pl-c">/* mute/unmute button*/</span></td>
      </tr>
      <tr>
        <td id="L655" class="blob-num js-line-number" data-line-number="655"></td>
        <td id="LC655" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> MuteUnmute <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span&gt;<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">mute</span> <span class="pl-k">+</span> <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;/span&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_muteUnmute vimeo_icon<span class="pl-pds">&quot;</span></span> ).<span class="pl-c1">click</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L656" class="blob-num js-line-number" data-line-number="656"></td>
        <td id="LC656" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L657" class="blob-num js-line-number" data-line-number="657"></td>
        <td id="LC657" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> ) {</td>
      </tr>
      <tr>
        <td id="L658" class="blob-num js-line-number" data-line-number="658"></td>
        <td id="LC658" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_unmute</span>();</td>
      </tr>
      <tr>
        <td id="L659" class="blob-num js-line-number" data-line-number="659"></td>
        <td id="LC659" class="blob-code blob-code-inner js-file-line">				} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L660" class="blob-num js-line-number" data-line-number="660"></td>
        <td id="LC660" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_mute</span>();</td>
      </tr>
      <tr>
        <td id="L661" class="blob-num js-line-number" data-line-number="661"></td>
        <td id="LC661" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L662" class="blob-num js-line-number" data-line-number="662"></td>
        <td id="LC662" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L663" class="blob-num js-line-number" data-line-number="663"></td>
        <td id="LC663" class="blob-code blob-code-inner js-file-line">			<span class="pl-c">/* volume bar*/</span></td>
      </tr>
      <tr>
        <td id="L664" class="blob-num js-line-number" data-line-number="664"></td>
        <td id="LC664" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> volumeBar <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_volume_bar<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L665" class="blob-num js-line-number" data-line-number="665"></td>
        <td id="LC665" class="blob-code blob-code-inner js-file-line">				display<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>inline-block<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L666" class="blob-num js-line-number" data-line-number="666"></td>
        <td id="LC666" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L667" class="blob-num js-line-number" data-line-number="667"></td>
        <td id="LC667" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">volumeBar</span> <span class="pl-k">=</span> volumeBar;</td>
      </tr>
      <tr>
        <td id="L668" class="blob-num js-line-number" data-line-number="668"></td>
        <td id="LC668" class="blob-code blob-code-inner js-file-line">			<span class="pl-c">/* time elapsed */</span></td>
      </tr>
      <tr>
        <td id="L669" class="blob-num js-line-number" data-line-number="669"></td>
        <td id="LC669" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> idx <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_time<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L670" class="blob-num js-line-number" data-line-number="670"></td>
        <td id="LC670" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vURL <span class="pl-k">=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>https://vimeo.com/<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">videoID</span>;</td>
      </tr>
      <tr>
        <td id="L671" class="blob-num js-line-number" data-line-number="671"></td>
        <td id="LC671" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L672" class="blob-num js-line-number" data-line-number="672"></td>
        <td id="LC672" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> movieUrl <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">logo</span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_url vimeo_icon<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">attr</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>title<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>view on Vimeo<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>click<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L673" class="blob-num js-line-number" data-line-number="673"></td>
        <td id="LC673" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L674" class="blob-num js-line-number" data-line-number="674"></td>
        <td id="LC674" class="blob-code blob-code-inner js-file-line">				<span class="pl-c"><span class="pl-c">//</span>				console.debug( vURL );</span></td>
      </tr>
      <tr>
        <td id="L675" class="blob-num js-line-number" data-line-number="675"></td>
        <td id="LC675" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L676" class="blob-num js-line-number" data-line-number="676"></td>
        <td id="LC676" class="blob-code blob-code-inner js-file-line">				<span class="pl-c1">window</span>.<span class="pl-c1">open</span>( vURL, <span class="pl-s"><span class="pl-pds">&quot;</span>viewOnVimeo<span class="pl-pds">&quot;</span></span> )</td>
      </tr>
      <tr>
        <td id="L677" class="blob-num js-line-number" data-line-number="677"></td>
        <td id="LC677" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L678" class="blob-num js-line-number" data-line-number="678"></td>
        <td id="LC678" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L679" class="blob-num js-line-number" data-line-number="679"></td>
        <td id="LC679" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> fullscreen <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;span/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">fullscreen</span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_fullscreen vimeo_icon<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>click<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L680" class="blob-num js-line-number" data-line-number="680"></td>
        <td id="LC680" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_fullscreen</span>( <span class="pl-smi">data</span>.<span class="pl-smi">realfullscreen</span> );</td>
      </tr>
      <tr>
        <td id="L681" class="blob-num js-line-number" data-line-number="681"></td>
        <td id="LC681" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L682" class="blob-num js-line-number" data-line-number="682"></td>
        <td id="LC682" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> progressBar <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_pogress<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span> ).<span class="pl-c1">click</span>( <span class="pl-k">function</span>( <span class="pl-smi">e</span> ) {</td>
      </tr>
      <tr>
        <td id="L683" class="blob-num js-line-number" data-line-number="683"></td>
        <td id="LC683" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">timeBar</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L684" class="blob-num js-line-number" data-line-number="684"></td>
        <td id="LC684" class="blob-code blob-code-inner js-file-line">					width<span class="pl-k">:</span> ( <span class="pl-smi">e</span>.<span class="pl-smi">clientX</span> <span class="pl-k">-</span> <span class="pl-smi">timeBar</span>.<span class="pl-en">offset</span>().<span class="pl-c1">left</span> )</td>
      </tr>
      <tr>
        <td id="L685" class="blob-num js-line-number" data-line-number="685"></td>
        <td id="LC685" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L686" class="blob-num js-line-number" data-line-number="686"></td>
        <td id="LC686" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">timeW</span> <span class="pl-k">=</span> <span class="pl-smi">e</span>.<span class="pl-smi">clientX</span> <span class="pl-k">-</span> <span class="pl-smi">timeBar</span>.<span class="pl-en">offset</span>().<span class="pl-c1">left</span>;</td>
      </tr>
      <tr>
        <td id="L687" class="blob-num js-line-number" data-line-number="687"></td>
        <td id="LC687" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_loaded<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L688" class="blob-num js-line-number" data-line-number="688"></td>
        <td id="LC688" class="blob-code blob-code-inner js-file-line">					width<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L689" class="blob-num js-line-number" data-line-number="689"></td>
        <td id="LC689" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L690" class="blob-num js-line-number" data-line-number="690"></td>
        <td id="LC690" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> totalTime <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">duration</span> );</td>
      </tr>
      <tr>
        <td id="L691" class="blob-num js-line-number" data-line-number="691"></td>
        <td id="LC691" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">goto</span> <span class="pl-k">=</span> ( <span class="pl-smi">timeBar</span>.<span class="pl-c1">outerWidth</span>() <span class="pl-k">*</span> totalTime ) <span class="pl-k">/</span> <span class="pl-smi">progressBar</span>.<span class="pl-c1">outerWidth</span>();</td>
      </tr>
      <tr>
        <td id="L692" class="blob-num js-line-number" data-line-number="692"></td>
        <td id="LC692" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L693" class="blob-num js-line-number" data-line-number="693"></td>
        <td id="LC693" class="blob-code blob-code-inner js-file-line">				<span class="pl-c"><span class="pl-c">//</span>				console.debug( vimeo_player.goto );</span></td>
      </tr>
      <tr>
        <td id="L694" class="blob-num js-line-number" data-line-number="694"></td>
        <td id="LC694" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L695" class="blob-num js-line-number" data-line-number="695"></td>
        <td id="LC695" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_seekTo</span>( <span class="pl-c1">parseFloat</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">goto</span> ) );</td>
      </tr>
      <tr>
        <td id="L696" class="blob-num js-line-number" data-line-number="696"></td>
        <td id="LC696" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_loaded<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L697" class="blob-num js-line-number" data-line-number="697"></td>
        <td id="LC697" class="blob-code blob-code-inner js-file-line">					width<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L698" class="blob-num js-line-number" data-line-number="698"></td>
        <td id="LC698" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L699" class="blob-num js-line-number" data-line-number="699"></td>
        <td id="LC699" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L700" class="blob-num js-line-number" data-line-number="700"></td>
        <td id="LC700" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> loadedBar <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_loaded<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L701" class="blob-num js-line-number" data-line-number="701"></td>
        <td id="LC701" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> timeBar <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_seek_bar<span class="pl-pds">&quot;</span></span> ).<span class="pl-en">css</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>position<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>absolute<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L702" class="blob-num js-line-number" data-line-number="702"></td>
        <td id="LC702" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">progressBar</span>.<span class="pl-c1">append</span>( loadedBar ).<span class="pl-c1">append</span>( timeBar );</td>
      </tr>
      <tr>
        <td id="L703" class="blob-num js-line-number" data-line-number="703"></td>
        <td id="LC703" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">buttonBar</span>.<span class="pl-c1">append</span>( playpause ).<span class="pl-c1">append</span>( MuteUnmute ).<span class="pl-c1">append</span>( volumeBar ).<span class="pl-c1">append</span>( idx );</td>
      </tr>
      <tr>
        <td id="L704" class="blob-num js-line-number" data-line-number="704"></td>
        <td id="LC704" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">data</span>.<span class="pl-smi">show_vimeo_logo</span> ) {</td>
      </tr>
      <tr>
        <td id="L705" class="blob-num js-line-number" data-line-number="705"></td>
        <td id="LC705" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">buttonBar</span>.<span class="pl-c1">append</span>( movieUrl );</td>
      </tr>
      <tr>
        <td id="L706" class="blob-num js-line-number" data-line-number="706"></td>
        <td id="LC706" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L707" class="blob-num js-line-number" data-line-number="707"></td>
        <td id="LC707" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> <span class="pl-k">||</span> ( <span class="pl-c1">eval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">realfullscreen</span> ) <span class="pl-k">&amp;&amp;</span> <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> ) ) <span class="pl-smi">buttonBar</span>.<span class="pl-c1">append</span>( fullscreen );</td>
      </tr>
      <tr>
        <td id="L708" class="blob-num js-line-number" data-line-number="708"></td>
        <td id="LC708" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span>.<span class="pl-c1">append</span>( buttonBar ).<span class="pl-c1">append</span>( progressBar );</td>
      </tr>
      <tr>
        <td id="L709" class="blob-num js-line-number" data-line-number="709"></td>
        <td id="LC709" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> ) {</td>
      </tr>
      <tr>
        <td id="L710" class="blob-num js-line-number" data-line-number="710"></td>
        <td id="LC710" class="blob-code blob-code-inner js-file-line">				<span class="pl-c"><span class="pl-c">//</span>vimeo_player.controlBar.addClass( &quot;inline_vimeo_player&quot; );</span></td>
      </tr>
      <tr>
        <td id="L711" class="blob-num js-line-number" data-line-number="711"></td>
        <td id="LC711" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>.<span class="pl-c1">before</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> );</td>
      </tr>
      <tr>
        <td id="L712" class="blob-num js-line-number" data-line-number="712"></td>
        <td id="LC712" class="blob-code blob-code-inner js-file-line">			} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L713" class="blob-num js-line-number" data-line-number="713"></td>
        <td id="LC713" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>body<span class="pl-pds">&quot;</span></span> ).<span class="pl-c1">after</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controlBar</span> );</td>
      </tr>
      <tr>
        <td id="L714" class="blob-num js-line-number" data-line-number="714"></td>
        <td id="LC714" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L715" class="blob-num js-line-number" data-line-number="715"></td>
        <td id="LC715" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L716" class="blob-num js-line-number" data-line-number="716"></td>
        <td id="LC716" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">volumeBar</span>.<span class="pl-en">simpleSlider</span>( {</td>
      </tr>
      <tr>
        <td id="L717" class="blob-num js-line-number" data-line-number="717"></td>
        <td id="LC717" class="blob-code blob-code-inner js-file-line">				initialval<span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span>,</td>
      </tr>
      <tr>
        <td id="L718" class="blob-num js-line-number" data-line-number="718"></td>
        <td id="LC718" class="blob-code blob-code-inner js-file-line">				scale<span class="pl-k">:</span> <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L719" class="blob-num js-line-number" data-line-number="719"></td>
        <td id="LC719" class="blob-code blob-code-inner js-file-line">				orientation<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span>,</td>
      </tr>
      <tr>
        <td id="L720" class="blob-num js-line-number" data-line-number="720"></td>
        <td id="LC720" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">callback</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">el</span> ) {</td>
      </tr>
      <tr>
        <td id="L721" class="blob-num js-line-number" data-line-number="721"></td>
        <td id="LC721" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-smi">el</span>.<span class="pl-c1">value</span> <span class="pl-k">==</span> <span class="pl-c1">0</span> ) {</td>
      </tr>
      <tr>
        <td id="L722" class="blob-num js-line-number" data-line-number="722"></td>
        <td id="LC722" class="blob-code blob-code-inner js-file-line">						<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_mute</span>();</td>
      </tr>
      <tr>
        <td id="L723" class="blob-num js-line-number" data-line-number="723"></td>
        <td id="LC723" class="blob-code blob-code-inner js-file-line">					} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L724" class="blob-num js-line-number" data-line-number="724"></td>
        <td id="LC724" class="blob-code blob-code-inner js-file-line">						<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_unmute</span>();</td>
      </tr>
      <tr>
        <td id="L725" class="blob-num js-line-number" data-line-number="725"></td>
        <td id="LC725" class="blob-code blob-code-inner js-file-line">					}</td>
      </tr>
      <tr>
        <td id="L726" class="blob-num js-line-number" data-line-number="726"></td>
        <td id="LC726" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">player</span>.<span class="pl-en">setVolume</span>( <span class="pl-smi">el</span>.<span class="pl-c1">value</span> <span class="pl-k">/</span> <span class="pl-c1">100</span> );</td>
      </tr>
      <tr>
        <td id="L727" class="blob-num js-line-number" data-line-number="727"></td>
        <td id="LC727" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L728" class="blob-num js-line-number" data-line-number="728"></td>
        <td id="LC728" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isMute</span> )</td>
      </tr>
      <tr>
        <td id="L729" class="blob-num js-line-number" data-line-number="729"></td>
        <td id="LC729" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">vol</span> <span class="pl-k">=</span> <span class="pl-smi">el</span>.<span class="pl-c1">value</span>;</td>
      </tr>
      <tr>
        <td id="L730" class="blob-num js-line-number" data-line-number="730"></td>
        <td id="LC730" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L731" class="blob-num js-line-number" data-line-number="731"></td>
        <td id="LC731" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L732" class="blob-num js-line-number" data-line-number="732"></td>
        <td id="LC732" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L733" class="blob-num js-line-number" data-line-number="733"></td>
        <td id="LC733" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L734" class="blob-num js-line-number" data-line-number="734"></td>
        <td id="LC734" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">optimizeVimeoDisplay</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">align</span> ) {</td>
      </tr>
      <tr>
        <td id="L735" class="blob-num js-line-number" data-line-number="735"></td>
        <td id="LC735" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L736" class="blob-num js-line-number" data-line-number="736"></td>
        <td id="LC736" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L737" class="blob-num js-line-number" data-line-number="737"></td>
        <td id="LC737" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vid <span class="pl-k">=</span> {};</td>
      </tr>
      <tr>
        <td id="L738" class="blob-num js-line-number" data-line-number="738"></td>
        <td id="LC738" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L739" class="blob-num js-line-number" data-line-number="739"></td>
        <td id="LC739" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span> <span class="pl-k">=</span> align <span class="pl-k">||</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span>;</td>
      </tr>
      <tr>
        <td id="L740" class="blob-num js-line-number" data-line-number="740"></td>
        <td id="LC740" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L741" class="blob-num js-line-number" data-line-number="741"></td>
        <td id="LC741" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span> <span class="pl-k">=</span> <span class="pl-k">typeof</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span> <span class="pl-k">!=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>undefined <span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span> <span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>center,center<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L742" class="blob-num js-line-number" data-line-number="742"></td>
        <td id="LC742" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> YTPAlign <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span>.<span class="pl-c1">split</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>,<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L743" class="blob-num js-line-number" data-line-number="743"></td>
        <td id="LC743" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L744" class="blob-num js-line-number" data-line-number="744"></td>
        <td id="LC744" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">optimizeDisplay</span> ) {</td>
      </tr>
      <tr>
        <td id="L745" class="blob-num js-line-number" data-line-number="745"></td>
        <td id="LC745" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> abundance <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isPlayer</span> <span class="pl-k">?</span> <span class="pl-c1">0</span> <span class="pl-k">:</span> <span class="pl-c1">80</span>;</td>
      </tr>
      <tr>
        <td id="L746" class="blob-num js-line-number" data-line-number="746"></td>
        <td id="LC746" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> win <span class="pl-k">=</span> {};</td>
      </tr>
      <tr>
        <td id="L747" class="blob-num js-line-number" data-line-number="747"></td>
        <td id="LC747" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> el <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>;</td>
      </tr>
      <tr>
        <td id="L748" class="blob-num js-line-number" data-line-number="748"></td>
        <td id="LC748" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L749" class="blob-num js-line-number" data-line-number="749"></td>
        <td id="LC749" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">win</span>.<span class="pl-c1">width</span> <span class="pl-k">=</span> <span class="pl-smi">el</span>.<span class="pl-c1">outerWidth</span>();</td>
      </tr>
      <tr>
        <td id="L750" class="blob-num js-line-number" data-line-number="750"></td>
        <td id="LC750" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">win</span>.<span class="pl-c1">height</span> <span class="pl-k">=</span> <span class="pl-smi">el</span>.<span class="pl-c1">outerHeight</span>() <span class="pl-k">+</span> abundance;</td>
      </tr>
      <tr>
        <td id="L751" class="blob-num js-line-number" data-line-number="751"></td>
        <td id="LC751" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L752" class="blob-num js-line-number" data-line-number="752"></td>
        <td id="LC752" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">=</span> <span class="pl-smi">win</span>.<span class="pl-c1">width</span>;</td>
      </tr>
      <tr>
        <td id="L753" class="blob-num js-line-number" data-line-number="753"></td>
        <td id="LC753" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L754" class="blob-num js-line-number" data-line-number="754"></td>
        <td id="LC754" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> <span class="pl-k">=</span> <span class="pl-c1">eval</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> )</td>
      </tr>
      <tr>
        <td id="L755" class="blob-num js-line-number" data-line-number="755"></td>
        <td id="LC755" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L756" class="blob-num js-line-number" data-line-number="756"></td>
        <td id="LC756" class="blob-code blob-code-inner js-file-line">				<span class="pl-c"><span class="pl-c">//</span>vid.height = vimeo_player.opt.ratio == &quot;16/9&quot; ? Math.ceil( vid.width * ( 9 / 16 ) ) : Math.ceil( vid.width * ( 3 / 4 ) );</span></td>
      </tr>
      <tr>
        <td id="L757" class="blob-num js-line-number" data-line-number="757"></td>
        <td id="LC757" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">ceil</span>( <span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">/</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> );</td>
      </tr>
      <tr>
        <td id="L758" class="blob-num js-line-number" data-line-number="758"></td>
        <td id="LC758" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L759" class="blob-num js-line-number" data-line-number="759"></td>
        <td id="LC759" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span> <span class="pl-k">=</span> <span class="pl-k">-</span>( ( <span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">height</span> ) <span class="pl-k">/</span> <span class="pl-c1">2</span> );</td>
      </tr>
      <tr>
        <td id="L760" class="blob-num js-line-number" data-line-number="760"></td>
        <td id="LC760" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L761" class="blob-num js-line-number" data-line-number="761"></td>
        <td id="LC761" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L762" class="blob-num js-line-number" data-line-number="762"></td>
        <td id="LC762" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> lowest <span class="pl-k">=</span> <span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">&lt;</span> <span class="pl-smi">win</span>.<span class="pl-c1">height</span>;</td>
      </tr>
      <tr>
        <td id="L763" class="blob-num js-line-number" data-line-number="763"></td>
        <td id="LC763" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L764" class="blob-num js-line-number" data-line-number="764"></td>
        <td id="LC764" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( lowest ) {</td>
      </tr>
      <tr>
        <td id="L765" class="blob-num js-line-number" data-line-number="765"></td>
        <td id="LC765" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L766" class="blob-num js-line-number" data-line-number="766"></td>
        <td id="LC766" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">=</span> <span class="pl-smi">win</span>.<span class="pl-c1">height</span> <span class="pl-k">+</span> abundance;</td>
      </tr>
      <tr>
        <td id="L767" class="blob-num js-line-number" data-line-number="767"></td>
        <td id="LC767" class="blob-code blob-code-inner js-file-line">					<span class="pl-c"><span class="pl-c">//</span>vid.width = vimeo_player.opt.ratio == &quot;16/9&quot; ? Math.floor( vid.height * ( 16 / 9 ) ) : Math.floor( vid.height * ( 4 / 3 ) );</span></td>
      </tr>
      <tr>
        <td id="L768" class="blob-num js-line-number" data-line-number="768"></td>
        <td id="LC768" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">=</span> <span class="pl-c1">Math</span>.<span class="pl-c1">ceil</span>( <span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">*</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">ratio</span> );</td>
      </tr>
      <tr>
        <td id="L769" class="blob-num js-line-number" data-line-number="769"></td>
        <td id="LC769" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L770" class="blob-num js-line-number" data-line-number="770"></td>
        <td id="LC770" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L771" class="blob-num js-line-number" data-line-number="771"></td>
        <td id="LC771" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> <span class="pl-k">-</span>( ( <span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">width</span> ) <span class="pl-k">/</span> <span class="pl-c1">2</span> );</td>
      </tr>
      <tr>
        <td id="L772" class="blob-num js-line-number" data-line-number="772"></td>
        <td id="LC772" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L773" class="blob-num js-line-number" data-line-number="773"></td>
        <td id="LC773" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L774" class="blob-num js-line-number" data-line-number="774"></td>
        <td id="LC774" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L775" class="blob-num js-line-number" data-line-number="775"></td>
        <td id="LC775" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">for</span>( <span class="pl-k">var</span> a <span class="pl-k">in</span> YTPAlign ) {</td>
      </tr>
      <tr>
        <td id="L776" class="blob-num js-line-number" data-line-number="776"></td>
        <td id="LC776" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L777" class="blob-num js-line-number" data-line-number="777"></td>
        <td id="LC777" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-smi">YTPAlign</span>.<span class="pl-en">hasOwnProperty</span>( a ) ) {</td>
      </tr>
      <tr>
        <td id="L778" class="blob-num js-line-number" data-line-number="778"></td>
        <td id="LC778" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L779" class="blob-num js-line-number" data-line-number="779"></td>
        <td id="LC779" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">var</span> al <span class="pl-k">=</span> YTPAlign[ a ].<span class="pl-c1">replace</span>(<span class="pl-sr"> <span class="pl-pds">/</span> <span class="pl-pds">/</span>g</span>, <span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L780" class="blob-num js-line-number" data-line-number="780"></td>
        <td id="LC780" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L781" class="blob-num js-line-number" data-line-number="781"></td>
        <td id="LC781" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">switch</span>( al ) {</td>
      </tr>
      <tr>
        <td id="L782" class="blob-num js-line-number" data-line-number="782"></td>
        <td id="LC782" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L783" class="blob-num js-line-number" data-line-number="783"></td>
        <td id="LC783" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">case</span> <span class="pl-s"><span class="pl-pds">&quot;</span>top<span class="pl-pds">&quot;</span></span>:</td>
      </tr>
      <tr>
        <td id="L784" class="blob-num js-line-number" data-line-number="784"></td>
        <td id="LC784" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span> <span class="pl-k">=</span> lowest <span class="pl-k">?</span> <span class="pl-k">-</span>( ( <span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">height</span> ) <span class="pl-k">/</span> <span class="pl-c1">2</span> ) <span class="pl-k">:</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L785" class="blob-num js-line-number" data-line-number="785"></td>
        <td id="LC785" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">break</span>;</td>
      </tr>
      <tr>
        <td id="L786" class="blob-num js-line-number" data-line-number="786"></td>
        <td id="LC786" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L787" class="blob-num js-line-number" data-line-number="787"></td>
        <td id="LC787" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">case</span> <span class="pl-s"><span class="pl-pds">&quot;</span>bottom<span class="pl-pds">&quot;</span></span>:</td>
      </tr>
      <tr>
        <td id="L788" class="blob-num js-line-number" data-line-number="788"></td>
        <td id="LC788" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span> <span class="pl-k">=</span> lowest <span class="pl-k">?</span> <span class="pl-c1">0</span> <span class="pl-k">:</span> <span class="pl-k">-</span>( <span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">height</span> );</td>
      </tr>
      <tr>
        <td id="L789" class="blob-num js-line-number" data-line-number="789"></td>
        <td id="LC789" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">break</span>;</td>
      </tr>
      <tr>
        <td id="L790" class="blob-num js-line-number" data-line-number="790"></td>
        <td id="LC790" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L791" class="blob-num js-line-number" data-line-number="791"></td>
        <td id="LC791" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">case</span> <span class="pl-s"><span class="pl-pds">&quot;</span>left<span class="pl-pds">&quot;</span></span>:</td>
      </tr>
      <tr>
        <td id="L792" class="blob-num js-line-number" data-line-number="792"></td>
        <td id="LC792" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L793" class="blob-num js-line-number" data-line-number="793"></td>
        <td id="LC793" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">break</span>;</td>
      </tr>
      <tr>
        <td id="L794" class="blob-num js-line-number" data-line-number="794"></td>
        <td id="LC794" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L795" class="blob-num js-line-number" data-line-number="795"></td>
        <td id="LC795" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">case</span> <span class="pl-s"><span class="pl-pds">&quot;</span>right<span class="pl-pds">&quot;</span></span>:</td>
      </tr>
      <tr>
        <td id="L796" class="blob-num js-line-number" data-line-number="796"></td>
        <td id="LC796" class="blob-code blob-code-inner js-file-line">								<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> lowest <span class="pl-k">?</span> <span class="pl-k">-</span>( <span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">width</span> ) <span class="pl-k">:</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L797" class="blob-num js-line-number" data-line-number="797"></td>
        <td id="LC797" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">break</span>;</td>
      </tr>
      <tr>
        <td id="L798" class="blob-num js-line-number" data-line-number="798"></td>
        <td id="LC798" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L799" class="blob-num js-line-number" data-line-number="799"></td>
        <td id="LC799" class="blob-code blob-code-inner js-file-line">							<span class="pl-k">default</span>:</td>
      </tr>
      <tr>
        <td id="L800" class="blob-num js-line-number" data-line-number="800"></td>
        <td id="LC800" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">if</span>( <span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">&gt;</span> <span class="pl-smi">win</span>.<span class="pl-c1">width</span> )</td>
      </tr>
      <tr>
        <td id="L801" class="blob-num js-line-number" data-line-number="801"></td>
        <td id="LC801" class="blob-code blob-code-inner js-file-line">									<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> <span class="pl-k">-</span>( ( <span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">-</span> <span class="pl-smi">win</span>.<span class="pl-c1">width</span> ) <span class="pl-k">/</span> <span class="pl-c1">2</span> );</td>
      </tr>
      <tr>
        <td id="L802" class="blob-num js-line-number" data-line-number="802"></td>
        <td id="LC802" class="blob-code blob-code-inner js-file-line">								<span class="pl-k">break</span>;</td>
      </tr>
      <tr>
        <td id="L803" class="blob-num js-line-number" data-line-number="803"></td>
        <td id="LC803" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L804" class="blob-num js-line-number" data-line-number="804"></td>
        <td id="LC804" class="blob-code blob-code-inner js-file-line">						}</td>
      </tr>
      <tr>
        <td id="L805" class="blob-num js-line-number" data-line-number="805"></td>
        <td id="LC805" class="blob-code blob-code-inner js-file-line">					}</td>
      </tr>
      <tr>
        <td id="L806" class="blob-num js-line-number" data-line-number="806"></td>
        <td id="LC806" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L807" class="blob-num js-line-number" data-line-number="807"></td>
        <td id="LC807" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L808" class="blob-num js-line-number" data-line-number="808"></td>
        <td id="LC808" class="blob-code blob-code-inner js-file-line">			} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L809" class="blob-num js-line-number" data-line-number="809"></td>
        <td id="LC809" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L810" class="blob-num js-line-number" data-line-number="810"></td>
        <td id="LC810" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">width</span> <span class="pl-k">=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L811" class="blob-num js-line-number" data-line-number="811"></td>
        <td id="LC811" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">height</span> <span class="pl-k">=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>100%<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L812" class="blob-num js-line-number" data-line-number="812"></td>
        <td id="LC812" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L813" class="blob-num js-line-number" data-line-number="813"></td>
        <td id="LC813" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span> <span class="pl-k">=</span> <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L814" class="blob-num js-line-number" data-line-number="814"></td>
        <td id="LC814" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L815" class="blob-num js-line-number" data-line-number="815"></td>
        <td id="LC815" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L816" class="blob-num js-line-number" data-line-number="816"></td>
        <td id="LC816" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L817" class="blob-num js-line-number" data-line-number="817"></td>
        <td id="LC817" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">playerBox</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L818" class="blob-num js-line-number" data-line-number="818"></td>
        <td id="LC818" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L819" class="blob-num js-line-number" data-line-number="819"></td>
        <td id="LC819" class="blob-code blob-code-inner js-file-line">				width<span class="pl-k">:</span> <span class="pl-smi">vid</span>.<span class="pl-c1">width</span>,</td>
      </tr>
      <tr>
        <td id="L820" class="blob-num js-line-number" data-line-number="820"></td>
        <td id="LC820" class="blob-code blob-code-inner js-file-line">				height<span class="pl-k">:</span> <span class="pl-smi">vid</span>.<span class="pl-c1">height</span>,</td>
      </tr>
      <tr>
        <td id="L821" class="blob-num js-line-number" data-line-number="821"></td>
        <td id="LC821" class="blob-code blob-code-inner js-file-line">				marginTop<span class="pl-k">:</span> <span class="pl-smi">vid</span>.<span class="pl-c1">marginTop</span>,</td>
      </tr>
      <tr>
        <td id="L822" class="blob-num js-line-number" data-line-number="822"></td>
        <td id="LC822" class="blob-code blob-code-inner js-file-line">				marginLeft<span class="pl-k">:</span> <span class="pl-smi">vid</span>.<span class="pl-c1">marginLeft</span>,</td>
      </tr>
      <tr>
        <td id="L823" class="blob-num js-line-number" data-line-number="823"></td>
        <td id="LC823" class="blob-code blob-code-inner js-file-line">				maxWidth<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>initial<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L824" class="blob-num js-line-number" data-line-number="824"></td>
        <td id="LC824" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L825" class="blob-num js-line-number" data-line-number="825"></td>
        <td id="LC825" class="blob-code blob-code-inner js-file-line">			} );</td>
      </tr>
      <tr>
        <td id="L826" class="blob-num js-line-number" data-line-number="826"></td>
        <td id="LC826" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L827" class="blob-num js-line-number" data-line-number="827"></td>
        <td id="LC827" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L828" class="blob-num js-line-number" data-line-number="828"></td>
        <td id="LC828" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L829" class="blob-num js-line-number" data-line-number="829"></td>
        <td id="LC829" class="blob-code blob-code-inner js-file-line">		<span class="pl-c">/**</span></td>
      </tr>
      <tr>
        <td id="L830" class="blob-num js-line-number" data-line-number="830"></td>
        <td id="LC830" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 *</span></td>
      </tr>
      <tr>
        <td id="L831" class="blob-num js-line-number" data-line-number="831"></td>
        <td id="LC831" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 * <span class="pl-k">@param</span> <span class="pl-smi">align</span></span></td>
      </tr>
      <tr>
        <td id="L832" class="blob-num js-line-number" data-line-number="832"></td>
        <td id="LC832" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 */</span></td>
      </tr>
      <tr>
        <td id="L833" class="blob-num js-line-number" data-line-number="833"></td>
        <td id="LC833" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">setAlign</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">align</span> ) {</td>
      </tr>
      <tr>
        <td id="L834" class="blob-num js-line-number" data-line-number="834"></td>
        <td id="LC834" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> $vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L835" class="blob-num js-line-number" data-line-number="835"></td>
        <td id="LC835" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L836" class="blob-num js-line-number" data-line-number="836"></td>
        <td id="LC836" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">v_optimize_display</span>( align );</td>
      </tr>
      <tr>
        <td id="L837" class="blob-num js-line-number" data-line-number="837"></td>
        <td id="LC837" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L838" class="blob-num js-line-number" data-line-number="838"></td>
        <td id="LC838" class="blob-code blob-code-inner js-file-line">		<span class="pl-c">/**</span></td>
      </tr>
      <tr>
        <td id="L839" class="blob-num js-line-number" data-line-number="839"></td>
        <td id="LC839" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 *</span></td>
      </tr>
      <tr>
        <td id="L840" class="blob-num js-line-number" data-line-number="840"></td>
        <td id="LC840" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 * <span class="pl-k">@param</span> <span class="pl-smi">align</span></span></td>
      </tr>
      <tr>
        <td id="L841" class="blob-num js-line-number" data-line-number="841"></td>
        <td id="LC841" class="blob-code blob-code-inner js-file-line"><span class="pl-c">		 */</span></td>
      </tr>
      <tr>
        <td id="L842" class="blob-num js-line-number" data-line-number="842"></td>
        <td id="LC842" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">getAlign</span><span class="pl-k">:</span> <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L843" class="blob-num js-line-number" data-line-number="843"></td>
        <td id="LC843" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L844" class="blob-num js-line-number" data-line-number="844"></td>
        <td id="LC844" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-c1">align</span>;</td>
      </tr>
      <tr>
        <td id="L845" class="blob-num js-line-number" data-line-number="845"></td>
        <td id="LC845" class="blob-code blob-code-inner js-file-line">		},</td>
      </tr>
      <tr>
        <td id="L846" class="blob-num js-line-number" data-line-number="846"></td>
        <td id="LC846" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L847" class="blob-num js-line-number" data-line-number="847"></td>
        <td id="LC847" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L848" class="blob-num js-line-number" data-line-number="848"></td>
        <td id="LC848" class="blob-code blob-code-inner js-file-line">		<span class="pl-en">fullscreen</span><span class="pl-k">:</span> <span class="pl-k">function</span>( <span class="pl-smi">real</span> ) {</td>
      </tr>
      <tr>
        <td id="L849" class="blob-num js-line-number" data-line-number="849"></td>
        <td id="LC849" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> vimeo_player <span class="pl-k">=</span> <span class="pl-c1">this</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> );</td>
      </tr>
      <tr>
        <td id="L850" class="blob-num js-line-number" data-line-number="850"></td>
        <td id="LC850" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> $vimeo_player <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( vimeo_player );</td>
      </tr>
      <tr>
        <td id="L851" class="blob-num js-line-number" data-line-number="851"></td>
        <td id="LC851" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> VEvent;</td>
      </tr>
      <tr>
        <td id="L852" class="blob-num js-line-number" data-line-number="852"></td>
        <td id="LC852" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L853" class="blob-num js-line-number" data-line-number="853"></td>
        <td id="LC853" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">typeof</span> real <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>undefined<span class="pl-pds">&quot;</span></span> ) real <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">realfullscreen</span>;</td>
      </tr>
      <tr>
        <td id="L854" class="blob-num js-line-number" data-line-number="854"></td>
        <td id="LC854" class="blob-code blob-code-inner js-file-line">			real <span class="pl-k">=</span> <span class="pl-c1">eval</span>( real );</td>
      </tr>
      <tr>
        <td id="L855" class="blob-num js-line-number" data-line-number="855"></td>
        <td id="LC855" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> controls <span class="pl-k">=</span> <span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>#controlBar_<span class="pl-pds">&quot;</span></span> <span class="pl-k">+</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-c1">id</span> );</td>
      </tr>
      <tr>
        <td id="L856" class="blob-num js-line-number" data-line-number="856"></td>
        <td id="LC856" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> fullScreenBtn <span class="pl-k">=</span> <span class="pl-smi">controls</span>.<span class="pl-c1">find</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_fullscreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L857" class="blob-num js-line-number" data-line-number="857"></td>
        <td id="LC857" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">var</span> videoWrapper <span class="pl-k">=</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isSelf</span> <span class="pl-k">?</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">containment</span> <span class="pl-k">:</span> <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>;</td>
      </tr>
      <tr>
        <td id="L858" class="blob-num js-line-number" data-line-number="858"></td>
        <td id="LC858" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L859" class="blob-num js-line-number" data-line-number="859"></td>
        <td id="LC859" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( real ) {</td>
      </tr>
      <tr>
        <td id="L860" class="blob-num js-line-number" data-line-number="860"></td>
        <td id="LC860" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> fullscreenchange <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mozilla</span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>mozfullscreenchange<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span> <span class="pl-k">?</span> <span class="pl-s"><span class="pl-pds">&quot;</span>webkitfullscreenchange<span class="pl-pds">&quot;</span></span> <span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>fullscreenchange<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L861" class="blob-num js-line-number" data-line-number="861"></td>
        <td id="LC861" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-c1">document</span> ).<span class="pl-en">off</span>( fullscreenchange ).<span class="pl-en">on</span>( fullscreenchange, <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L862" class="blob-num js-line-number" data-line-number="862"></td>
        <td id="LC862" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">var</span> isFullScreen <span class="pl-k">=</span> <span class="pl-en">RunPrefixMethod</span>( <span class="pl-c1">document</span>, <span class="pl-s"><span class="pl-pds">&quot;</span>IsFullScreen<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">||</span> <span class="pl-en">RunPrefixMethod</span>( <span class="pl-c1">document</span>, <span class="pl-s"><span class="pl-pds">&quot;</span>FullScreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L863" class="blob-num js-line-number" data-line-number="863"></td>
        <td id="LC863" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-k">!</span>isFullScreen ) {</td>
      </tr>
      <tr>
        <td id="L864" class="blob-num js-line-number" data-line-number="864"></td>
        <td id="LC864" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isAlone</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L865" class="blob-num js-line-number" data-line-number="865"></td>
        <td id="LC865" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">fullScreenBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">fullscreen</span> );</td>
      </tr>
      <tr>
        <td id="L866" class="blob-num js-line-number" data-line-number="866"></td>
        <td id="LC866" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">videoWrapper</span>.<span class="pl-en">removeClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_Fullscreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L867" class="blob-num js-line-number" data-line-number="867"></td>
        <td id="LC867" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L868" class="blob-num js-line-number" data-line-number="868"></td>
        <td id="LC868" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">videoWrapper</span>.<span class="pl-en">fadeTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">opacity</span> );</td>
      </tr>
      <tr>
        <td id="L869" class="blob-num js-line-number" data-line-number="869"></td>
        <td id="LC869" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L870" class="blob-num js-line-number" data-line-number="870"></td>
        <td id="LC870" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">videoWrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L871" class="blob-num js-line-number" data-line-number="871"></td>
        <td id="LC871" class="blob-code blob-code-inner js-file-line">							zIndex<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L872" class="blob-num js-line-number" data-line-number="872"></td>
        <td id="LC872" class="blob-code blob-code-inner js-file-line">						} );</td>
      </tr>
      <tr>
        <td id="L873" class="blob-num js-line-number" data-line-number="873"></td>
        <td id="LC873" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L874" class="blob-num js-line-number" data-line-number="874"></td>
        <td id="LC874" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">if</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isBackground</span> ) {</td>
      </tr>
      <tr>
        <td id="L875" class="blob-num js-line-number" data-line-number="875"></td>
        <td id="LC875" class="blob-code blob-code-inner js-file-line">							<span class="pl-en">jQuery</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>body<span class="pl-pds">&quot;</span></span> ).<span class="pl-c1">after</span>( controls );</td>
      </tr>
      <tr>
        <td id="L876" class="blob-num js-line-number" data-line-number="876"></td>
        <td id="LC876" class="blob-code blob-code-inner js-file-line">						} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L877" class="blob-num js-line-number" data-line-number="877"></td>
        <td id="LC877" class="blob-code blob-code-inner js-file-line">							<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>.<span class="pl-c1">before</span>( controls );</td>
      </tr>
      <tr>
        <td id="L878" class="blob-num js-line-number" data-line-number="878"></td>
        <td id="LC878" class="blob-code blob-code-inner js-file-line">						}</td>
      </tr>
      <tr>
        <td id="L879" class="blob-num js-line-number" data-line-number="879"></td>
        <td id="LC879" class="blob-code blob-code-inner js-file-line">						<span class="pl-en">jQuery</span>( <span class="pl-c1">window</span> ).<span class="pl-c1">resize</span>();</td>
      </tr>
      <tr>
        <td id="L880" class="blob-num js-line-number" data-line-number="880"></td>
        <td id="LC880" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span> Trigger state events</span></td>
      </tr>
      <tr>
        <td id="L881" class="blob-num js-line-number" data-line-number="881"></td>
        <td id="LC881" class="blob-code blob-code-inner js-file-line">						VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPFullScreenEnd<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L882" class="blob-num js-line-number" data-line-number="882"></td>
        <td id="LC882" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L883" class="blob-num js-line-number" data-line-number="883"></td>
        <td id="LC883" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L884" class="blob-num js-line-number" data-line-number="884"></td>
        <td id="LC884" class="blob-code blob-code-inner js-file-line">					} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L885" class="blob-num js-line-number" data-line-number="885"></td>
        <td id="LC885" class="blob-code blob-code-inner js-file-line">						<span class="pl-c"><span class="pl-c">//</span> Trigger state events</span></td>
      </tr>
      <tr>
        <td id="L886" class="blob-num js-line-number" data-line-number="886"></td>
        <td id="LC886" class="blob-code blob-code-inner js-file-line">						VEvent <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-en">Event</span>( <span class="pl-s"><span class="pl-pds">&#39;</span>VPFullScreenStart<span class="pl-pds">&#39;</span></span> );</td>
      </tr>
      <tr>
        <td id="L887" class="blob-num js-line-number" data-line-number="887"></td>
        <td id="LC887" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">$vimeo_player</span>.<span class="pl-en">trigger</span>( VEvent );</td>
      </tr>
      <tr>
        <td id="L888" class="blob-num js-line-number" data-line-number="888"></td>
        <td id="LC888" class="blob-code blob-code-inner js-file-line">					}</td>
      </tr>
      <tr>
        <td id="L889" class="blob-num js-line-number" data-line-number="889"></td>
        <td id="LC889" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L890" class="blob-num js-line-number" data-line-number="890"></td>
        <td id="LC890" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L891" class="blob-num js-line-number" data-line-number="891"></td>
        <td id="LC891" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isAlone</span> ) {</td>
      </tr>
      <tr>
        <td id="L892" class="blob-num js-line-number" data-line-number="892"></td>
        <td id="LC892" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">function</span> <span class="pl-en">hideMouse</span>() {</td>
      </tr>
      <tr>
        <td id="L893" class="blob-num js-line-number" data-line-number="893"></td>
        <td id="LC893" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L894" class="blob-num js-line-number" data-line-number="894"></td>
        <td id="LC894" class="blob-code blob-code-inner js-file-line">						cursor<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>none<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L895" class="blob-num js-line-number" data-line-number="895"></td>
        <td id="LC895" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L896" class="blob-num js-line-number" data-line-number="896"></td>
        <td id="LC896" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L897" class="blob-num js-line-number" data-line-number="897"></td>
        <td id="LC897" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L898" class="blob-num js-line-number" data-line-number="898"></td>
        <td id="LC898" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-c1">document</span> ).<span class="pl-en">on</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>mousemove.vimeo_player<span class="pl-pds">&quot;</span></span>, <span class="pl-k">function</span>( <span class="pl-smi">e</span> ) {</td>
      </tr>
      <tr>
        <td id="L899" class="blob-num js-line-number" data-line-number="899"></td>
        <td id="LC899" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L900" class="blob-num js-line-number" data-line-number="900"></td>
        <td id="LC900" class="blob-code blob-code-inner js-file-line">						cursor<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L901" class="blob-num js-line-number" data-line-number="901"></td>
        <td id="LC901" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L902" class="blob-num js-line-number" data-line-number="902"></td>
        <td id="LC902" class="blob-code blob-code-inner js-file-line">					<span class="pl-c1">clearTimeout</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">hideCursor</span> );</td>
      </tr>
      <tr>
        <td id="L903" class="blob-num js-line-number" data-line-number="903"></td>
        <td id="LC903" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( <span class="pl-k">!</span><span class="pl-en">jQuery</span>( <span class="pl-smi">e</span>.<span class="pl-c1">target</span> ).<span class="pl-en">parents</span>().<span class="pl-en">is</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>.vimeo_player_bar<span class="pl-pds">&quot;</span></span> ) )</td>
      </tr>
      <tr>
        <td id="L904" class="blob-num js-line-number" data-line-number="904"></td>
        <td id="LC904" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">hideCursor</span> <span class="pl-k">=</span> <span class="pl-c1">setTimeout</span>( hideMouse, <span class="pl-c1">3000</span> );</td>
      </tr>
      <tr>
        <td id="L905" class="blob-num js-line-number" data-line-number="905"></td>
        <td id="LC905" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L906" class="blob-num js-line-number" data-line-number="906"></td>
        <td id="LC906" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L907" class="blob-num js-line-number" data-line-number="907"></td>
        <td id="LC907" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">hideMouse</span>();</td>
      </tr>
      <tr>
        <td id="L908" class="blob-num js-line-number" data-line-number="908"></td>
        <td id="LC908" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L909" class="blob-num js-line-number" data-line-number="909"></td>
        <td id="LC909" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( real ) {</td>
      </tr>
      <tr>
        <td id="L910" class="blob-num js-line-number" data-line-number="910"></td>
        <td id="LC910" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">videoWrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L911" class="blob-num js-line-number" data-line-number="911"></td>
        <td id="LC911" class="blob-code blob-code-inner js-file-line">						opacity<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L912" class="blob-num js-line-number" data-line-number="912"></td>
        <td id="LC912" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L913" class="blob-num js-line-number" data-line-number="913"></td>
        <td id="LC913" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">videoWrapper</span>.<span class="pl-en">addClass</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>vimeo_player_Fullscreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L914" class="blob-num js-line-number" data-line-number="914"></td>
        <td id="LC914" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">launchFullscreen</span>( <span class="pl-smi">videoWrapper</span>.<span class="pl-c1">get</span>( <span class="pl-c1">0</span> ) );</td>
      </tr>
      <tr>
        <td id="L915" class="blob-num js-line-number" data-line-number="915"></td>
        <td id="LC915" class="blob-code blob-code-inner js-file-line">					<span class="pl-c1">setTimeout</span>( <span class="pl-k">function</span>() {</td>
      </tr>
      <tr>
        <td id="L916" class="blob-num js-line-number" data-line-number="916"></td>
        <td id="LC916" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">videoWrapper</span>.<span class="pl-en">fadeTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L917" class="blob-num js-line-number" data-line-number="917"></td>
        <td id="LC917" class="blob-code blob-code-inner js-file-line">						<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">wrapper</span>.<span class="pl-c1">append</span>( controls );</td>
      </tr>
      <tr>
        <td id="L918" class="blob-num js-line-number" data-line-number="918"></td>
        <td id="LC918" class="blob-code blob-code-inner js-file-line">						<span class="pl-en">jQuery</span>( vimeo_player ).<span class="pl-en">v_optimize_display</span>();</td>
      </tr>
      <tr>
        <td id="L919" class="blob-num js-line-number" data-line-number="919"></td>
        <td id="LC919" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L920" class="blob-num js-line-number" data-line-number="920"></td>
        <td id="LC920" class="blob-code blob-code-inner js-file-line">					}, <span class="pl-c1">500</span> )</td>
      </tr>
      <tr>
        <td id="L921" class="blob-num js-line-number" data-line-number="921"></td>
        <td id="LC921" class="blob-code blob-code-inner js-file-line">				} <span class="pl-k">else</span> <span class="pl-smi">videoWrapper</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L922" class="blob-num js-line-number" data-line-number="922"></td>
        <td id="LC922" class="blob-code blob-code-inner js-file-line">					zIndex<span class="pl-k">:</span> <span class="pl-c1">10000</span></td>
      </tr>
      <tr>
        <td id="L923" class="blob-num js-line-number" data-line-number="923"></td>
        <td id="LC923" class="blob-code blob-code-inner js-file-line">				} ).<span class="pl-en">fadeTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L924" class="blob-num js-line-number" data-line-number="924"></td>
        <td id="LC924" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">fullScreenBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">showSite</span> );</td>
      </tr>
      <tr>
        <td id="L925" class="blob-num js-line-number" data-line-number="925"></td>
        <td id="LC925" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isAlone</span> <span class="pl-k">=</span> <span class="pl-c1">true</span>;</td>
      </tr>
      <tr>
        <td id="L926" class="blob-num js-line-number" data-line-number="926"></td>
        <td id="LC926" class="blob-code blob-code-inner js-file-line">			} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L927" class="blob-num js-line-number" data-line-number="927"></td>
        <td id="LC927" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">jQuery</span>( <span class="pl-c1">document</span> ).<span class="pl-en">off</span>( <span class="pl-s"><span class="pl-pds">&quot;</span>mousemove.vimeo_player<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L928" class="blob-num js-line-number" data-line-number="928"></td>
        <td id="LC928" class="blob-code blob-code-inner js-file-line">				<span class="pl-c1">clearTimeout</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">hideCursor</span> );</td>
      </tr>
      <tr>
        <td id="L929" class="blob-num js-line-number" data-line-number="929"></td>
        <td id="LC929" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">overlay</span>.<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L930" class="blob-num js-line-number" data-line-number="930"></td>
        <td id="LC930" class="blob-code blob-code-inner js-file-line">					cursor<span class="pl-k">:</span> <span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L931" class="blob-num js-line-number" data-line-number="931"></td>
        <td id="LC931" class="blob-code blob-code-inner js-file-line">				} );</td>
      </tr>
      <tr>
        <td id="L932" class="blob-num js-line-number" data-line-number="932"></td>
        <td id="LC932" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( real ) {</td>
      </tr>
      <tr>
        <td id="L933" class="blob-num js-line-number" data-line-number="933"></td>
        <td id="LC933" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">cancelFullscreen</span>();</td>
      </tr>
      <tr>
        <td id="L934" class="blob-num js-line-number" data-line-number="934"></td>
        <td id="LC934" class="blob-code blob-code-inner js-file-line">				} <span class="pl-k">else</span> {</td>
      </tr>
      <tr>
        <td id="L935" class="blob-num js-line-number" data-line-number="935"></td>
        <td id="LC935" class="blob-code blob-code-inner js-file-line">					<span class="pl-smi">videoWrapper</span>.<span class="pl-en">fadeTo</span>( <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">fadeTime</span>, <span class="pl-smi">vimeo_player</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">opacity</span> ).<span class="pl-en">css</span>( {</td>
      </tr>
      <tr>
        <td id="L936" class="blob-num js-line-number" data-line-number="936"></td>
        <td id="LC936" class="blob-code blob-code-inner js-file-line">						zIndex<span class="pl-k">:</span> <span class="pl-c1">0</span></td>
      </tr>
      <tr>
        <td id="L937" class="blob-num js-line-number" data-line-number="937"></td>
        <td id="LC937" class="blob-code blob-code-inner js-file-line">					} );</td>
      </tr>
      <tr>
        <td id="L938" class="blob-num js-line-number" data-line-number="938"></td>
        <td id="LC938" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L939" class="blob-num js-line-number" data-line-number="939"></td>
        <td id="LC939" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">fullScreenBtn</span>.<span class="pl-en">html</span>( <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">controls</span>.<span class="pl-smi">fullscreen</span> );</td>
      </tr>
      <tr>
        <td id="L940" class="blob-num js-line-number" data-line-number="940"></td>
        <td id="LC940" class="blob-code blob-code-inner js-file-line">				<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">isAlone</span> <span class="pl-k">=</span> <span class="pl-c1">false</span>;</td>
      </tr>
      <tr>
        <td id="L941" class="blob-num js-line-number" data-line-number="941"></td>
        <td id="LC941" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L942" class="blob-num js-line-number" data-line-number="942"></td>
        <td id="LC942" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L943" class="blob-num js-line-number" data-line-number="943"></td>
        <td id="LC943" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">function</span> <span class="pl-en">RunPrefixMethod</span>( <span class="pl-smi">obj</span>, <span class="pl-smi">method</span> ) {</td>
      </tr>
      <tr>
        <td id="L944" class="blob-num js-line-number" data-line-number="944"></td>
        <td id="LC944" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> pfx <span class="pl-k">=</span> [ <span class="pl-s"><span class="pl-pds">&quot;</span>webkit<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>moz<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>ms<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span>o<span class="pl-pds">&quot;</span></span>, <span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span> ];</td>
      </tr>
      <tr>
        <td id="L945" class="blob-num js-line-number" data-line-number="945"></td>
        <td id="LC945" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">var</span> p <span class="pl-k">=</span> <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L946" class="blob-num js-line-number" data-line-number="946"></td>
        <td id="LC946" class="blob-code blob-code-inner js-file-line">					m, t;</td>
      </tr>
      <tr>
        <td id="L947" class="blob-num js-line-number" data-line-number="947"></td>
        <td id="LC947" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">while</span>( p <span class="pl-k">&lt;</span> <span class="pl-smi">pfx</span>.<span class="pl-c1">length</span> <span class="pl-k">&amp;&amp;</span> <span class="pl-k">!</span>obj[ m ] ) {</td>
      </tr>
      <tr>
        <td id="L948" class="blob-num js-line-number" data-line-number="948"></td>
        <td id="LC948" class="blob-code blob-code-inner js-file-line">					m <span class="pl-k">=</span> method;</td>
      </tr>
      <tr>
        <td id="L949" class="blob-num js-line-number" data-line-number="949"></td>
        <td id="LC949" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( pfx[ p ] <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span> ) {</td>
      </tr>
      <tr>
        <td id="L950" class="blob-num js-line-number" data-line-number="950"></td>
        <td id="LC950" class="blob-code blob-code-inner js-file-line">						m <span class="pl-k">=</span> <span class="pl-smi">m</span>.<span class="pl-c1">substr</span>( <span class="pl-c1">0</span>, <span class="pl-c1">1</span> ).<span class="pl-c1">toLowerCase</span>() <span class="pl-k">+</span> <span class="pl-smi">m</span>.<span class="pl-c1">substr</span>( <span class="pl-c1">1</span> );</td>
      </tr>
      <tr>
        <td id="L951" class="blob-num js-line-number" data-line-number="951"></td>
        <td id="LC951" class="blob-code blob-code-inner js-file-line">					}</td>
      </tr>
      <tr>
        <td id="L952" class="blob-num js-line-number" data-line-number="952"></td>
        <td id="LC952" class="blob-code blob-code-inner js-file-line">					m <span class="pl-k">=</span> pfx[ p ] <span class="pl-k">+</span> m;</td>
      </tr>
      <tr>
        <td id="L953" class="blob-num js-line-number" data-line-number="953"></td>
        <td id="LC953" class="blob-code blob-code-inner js-file-line">					t <span class="pl-k">=</span> <span class="pl-k">typeof</span> obj[ m ];</td>
      </tr>
      <tr>
        <td id="L954" class="blob-num js-line-number" data-line-number="954"></td>
        <td id="LC954" class="blob-code blob-code-inner js-file-line">					<span class="pl-k">if</span>( t <span class="pl-k">!=</span> <span class="pl-s"><span class="pl-pds">&quot;</span>undefined<span class="pl-pds">&quot;</span></span> ) {</td>
      </tr>
      <tr>
        <td id="L955" class="blob-num js-line-number" data-line-number="955"></td>
        <td id="LC955" class="blob-code blob-code-inner js-file-line">						pfx <span class="pl-k">=</span> [ pfx[ p ] ];</td>
      </tr>
      <tr>
        <td id="L956" class="blob-num js-line-number" data-line-number="956"></td>
        <td id="LC956" class="blob-code blob-code-inner js-file-line">						<span class="pl-k">return</span>( t <span class="pl-k">==</span> <span class="pl-s"><span class="pl-pds">&quot;</span>function<span class="pl-pds">&quot;</span></span> <span class="pl-k">?</span> obj[ m ]() <span class="pl-k">:</span> obj[ m ] );</td>
      </tr>
      <tr>
        <td id="L957" class="blob-num js-line-number" data-line-number="957"></td>
        <td id="LC957" class="blob-code blob-code-inner js-file-line">					}</td>
      </tr>
      <tr>
        <td id="L958" class="blob-num js-line-number" data-line-number="958"></td>
        <td id="LC958" class="blob-code blob-code-inner js-file-line">					p<span class="pl-k">++</span>;</td>
      </tr>
      <tr>
        <td id="L959" class="blob-num js-line-number" data-line-number="959"></td>
        <td id="LC959" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L960" class="blob-num js-line-number" data-line-number="960"></td>
        <td id="LC960" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L961" class="blob-num js-line-number" data-line-number="961"></td>
        <td id="LC961" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L962" class="blob-num js-line-number" data-line-number="962"></td>
        <td id="LC962" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">function</span> <span class="pl-en">launchFullscreen</span>( <span class="pl-smi">element</span> ) {</td>
      </tr>
      <tr>
        <td id="L963" class="blob-num js-line-number" data-line-number="963"></td>
        <td id="LC963" class="blob-code blob-code-inner js-file-line">				<span class="pl-en">RunPrefixMethod</span>( element, <span class="pl-s"><span class="pl-pds">&quot;</span>RequestFullScreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L964" class="blob-num js-line-number" data-line-number="964"></td>
        <td id="LC964" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L965" class="blob-num js-line-number" data-line-number="965"></td>
        <td id="LC965" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L966" class="blob-num js-line-number" data-line-number="966"></td>
        <td id="LC966" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">function</span> <span class="pl-en">cancelFullscreen</span>() {</td>
      </tr>
      <tr>
        <td id="L967" class="blob-num js-line-number" data-line-number="967"></td>
        <td id="LC967" class="blob-code blob-code-inner js-file-line">				<span class="pl-k">if</span>( <span class="pl-en">RunPrefixMethod</span>( <span class="pl-c1">document</span>, <span class="pl-s"><span class="pl-pds">&quot;</span>FullScreen<span class="pl-pds">&quot;</span></span> ) <span class="pl-k">||</span> <span class="pl-en">RunPrefixMethod</span>( <span class="pl-c1">document</span>, <span class="pl-s"><span class="pl-pds">&quot;</span>IsFullScreen<span class="pl-pds">&quot;</span></span> ) ) {</td>
      </tr>
      <tr>
        <td id="L968" class="blob-num js-line-number" data-line-number="968"></td>
        <td id="LC968" class="blob-code blob-code-inner js-file-line">					<span class="pl-en">RunPrefixMethod</span>( <span class="pl-c1">document</span>, <span class="pl-s"><span class="pl-pds">&quot;</span>CancelFullScreen<span class="pl-pds">&quot;</span></span> );</td>
      </tr>
      <tr>
        <td id="L969" class="blob-num js-line-number" data-line-number="969"></td>
        <td id="LC969" class="blob-code blob-code-inner js-file-line">				}</td>
      </tr>
      <tr>
        <td id="L970" class="blob-num js-line-number" data-line-number="970"></td>
        <td id="LC970" class="blob-code blob-code-inner js-file-line">			}</td>
      </tr>
      <tr>
        <td id="L971" class="blob-num js-line-number" data-line-number="971"></td>
        <td id="LC971" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L972" class="blob-num js-line-number" data-line-number="972"></td>
        <td id="LC972" class="blob-code blob-code-inner js-file-line">			<span class="pl-k">return</span> <span class="pl-c1">this</span>;</td>
      </tr>
      <tr>
        <td id="L973" class="blob-num js-line-number" data-line-number="973"></td>
        <td id="LC973" class="blob-code blob-code-inner js-file-line">		}</td>
      </tr>
      <tr>
        <td id="L974" class="blob-num js-line-number" data-line-number="974"></td>
        <td id="LC974" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L975" class="blob-num js-line-number" data-line-number="975"></td>
        <td id="LC975" class="blob-code blob-code-inner js-file-line">	};</td>
      </tr>
      <tr>
        <td id="L976" class="blob-num js-line-number" data-line-number="976"></td>
        <td id="LC976" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L977" class="blob-num js-line-number" data-line-number="977"></td>
        <td id="LC977" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">vimeo_player</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">buildPlayer</span>;</td>
      </tr>
      <tr>
        <td id="L978" class="blob-num js-line-number" data-line-number="978"></td>
        <td id="LC978" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_play</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">play</span>;</td>
      </tr>
      <tr>
        <td id="L979" class="blob-num js-line-number" data-line-number="979"></td>
        <td id="LC979" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_toggle_play</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">togglePlay</span>;</td>
      </tr>
      <tr>
        <td id="L980" class="blob-num js-line-number" data-line-number="980"></td>
        <td id="LC980" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_change_movie</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">changeMovie</span>;</td>
      </tr>
      <tr>
        <td id="L981" class="blob-num js-line-number" data-line-number="981"></td>
        <td id="LC981" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_pause</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">pause</span>;</td>
      </tr>
      <tr>
        <td id="L982" class="blob-num js-line-number" data-line-number="982"></td>
        <td id="LC982" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_seekTo</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">seekTo</span>;</td>
      </tr>
      <tr>
        <td id="L983" class="blob-num js-line-number" data-line-number="983"></td>
        <td id="LC983" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_optimize_display</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">optimizeVimeoDisplay</span>;</td>
      </tr>
      <tr>
        <td id="L984" class="blob-num js-line-number" data-line-number="984"></td>
        <td id="LC984" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_set_align</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">setAlign</span>;</td>
      </tr>
      <tr>
        <td id="L985" class="blob-num js-line-number" data-line-number="985"></td>
        <td id="LC985" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_get_align</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">getAlign</span>;</td>
      </tr>
      <tr>
        <td id="L986" class="blob-num js-line-number" data-line-number="986"></td>
        <td id="LC986" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_fullscreen</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">fullscreen</span>;</td>
      </tr>
      <tr>
        <td id="L987" class="blob-num js-line-number" data-line-number="987"></td>
        <td id="LC987" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_mute</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">mute</span>;</td>
      </tr>
      <tr>
        <td id="L988" class="blob-num js-line-number" data-line-number="988"></td>
        <td id="LC988" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_unmute</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">unmute</span>;</td>
      </tr>
      <tr>
        <td id="L989" class="blob-num js-line-number" data-line-number="989"></td>
        <td id="LC989" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_set_volume</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">setVolume</span>;</td>
      </tr>
      <tr>
        <td id="L990" class="blob-num js-line-number" data-line-number="990"></td>
        <td id="LC990" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">jQuery</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">v_toggle_volume</span> <span class="pl-k">=</span> <span class="pl-smi">jQuery</span>.<span class="pl-smi">vimeo_player</span>.<span class="pl-smi">toggleVolume</span>;</td>
      </tr>
      <tr>
        <td id="L991" class="blob-num js-line-number" data-line-number="991"></td>
        <td id="LC991" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L992" class="blob-num js-line-number" data-line-number="992"></td>
        <td id="LC992" class="blob-code blob-code-inner js-file-line">} )( jQuery );</td>
      </tr>
      <tr>
        <td id="L993" class="blob-num js-line-number" data-line-number="993"></td>
        <td id="LC993" class="blob-code blob-code-inner js-file-line">;<span class="pl-c">/*___________________________________________________________________________________________________________________________________________________</span></td>
      </tr>
      <tr>
        <td id="L994" class="blob-num js-line-number" data-line-number="994"></td>
        <td id="LC994" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ jquery.mb.components                                                                                                                             _</span></td>
      </tr>
      <tr>
        <td id="L995" class="blob-num js-line-number" data-line-number="995"></td>
        <td id="LC995" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L996" class="blob-num js-line-number" data-line-number="996"></td>
        <td id="LC996" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ file: jquery.mb.browser.min.js                                                                                                                   _</span></td>
      </tr>
      <tr>
        <td id="L997" class="blob-num js-line-number" data-line-number="997"></td>
        <td id="LC997" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ last modified: 07/06/16 22.34                                                                                                                    _</span></td>
      </tr>
      <tr>
        <td id="L998" class="blob-num js-line-number" data-line-number="998"></td>
        <td id="LC998" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L999" class="blob-num js-line-number" data-line-number="999"></td>
        <td id="LC999" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Open Lab s.r.l., Florence - Italy                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L1000" class="blob-num js-line-number" data-line-number="1000"></td>
        <td id="LC1000" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1001" class="blob-num js-line-number" data-line-number="1001"></td>
        <td id="LC1001" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ email: matteo@open-lab.com                                                                                                                       _</span></td>
      </tr>
      <tr>
        <td id="L1002" class="blob-num js-line-number" data-line-number="1002"></td>
        <td id="LC1002" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ site: http://pupunzi.com                                                                                                                         _</span></td>
      </tr>
      <tr>
        <td id="L1003" class="blob-num js-line-number" data-line-number="1003"></td>
        <td id="LC1003" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _       http://open-lab.com                                                                                                                        _</span></td>
      </tr>
      <tr>
        <td id="L1004" class="blob-num js-line-number" data-line-number="1004"></td>
        <td id="LC1004" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ blog: http://pupunzi.open-lab.com                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L1005" class="blob-num js-line-number" data-line-number="1005"></td>
        <td id="LC1005" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Q&amp;A:  http://jquery.pupunzi.com                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1006" class="blob-num js-line-number" data-line-number="1006"></td>
        <td id="LC1006" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1007" class="blob-num js-line-number" data-line-number="1007"></td>
        <td id="LC1007" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Licences: MIT, GPL                                                                                                                               _</span></td>
      </tr>
      <tr>
        <td id="L1008" class="blob-num js-line-number" data-line-number="1008"></td>
        <td id="LC1008" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.opensource.org/licenses/mit-license.php                                                                                            _</span></td>
      </tr>
      <tr>
        <td id="L1009" class="blob-num js-line-number" data-line-number="1009"></td>
        <td id="LC1009" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.gnu.org/licenses/gpl.html                                                                                                          _</span></td>
      </tr>
      <tr>
        <td id="L1010" class="blob-num js-line-number" data-line-number="1010"></td>
        <td id="LC1010" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1011" class="blob-num js-line-number" data-line-number="1011"></td>
        <td id="LC1011" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Copyright (c) 2001-2016. Matteo Bicocchi (Pupunzi);                                                                                              _</span></td>
      </tr>
      <tr>
        <td id="L1012" class="blob-num js-line-number" data-line-number="1012"></td>
        <td id="LC1012" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> ___________________________________________________________________________________________________________________________________________________*/</span></td>
      </tr>
      <tr>
        <td id="L1013" class="blob-num js-line-number" data-line-number="1013"></td>
        <td id="LC1013" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1014" class="blob-num js-line-number" data-line-number="1014"></td>
        <td id="LC1014" class="blob-code blob-code-inner js-file-line"><span class="pl-k">var</span> nAgt<span class="pl-k">=</span><span class="pl-c1">navigator</span>.<span class="pl-c1">userAgent</span>;</td>
      </tr>
      <tr>
        <td id="L1015" class="blob-num js-line-number" data-line-number="1015"></td>
        <td id="LC1015" class="blob-code blob-code-inner js-file-line"><span class="pl-k">if</span>(<span class="pl-k">!</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>){<span class="pl-k">var</span> <span class="pl-en">isTouchSupported</span><span class="pl-k">=</span><span class="pl-k">function</span>(){<span class="pl-k">var</span> a<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-smi">msMaxTouchPoints</span>,b<span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>ontouchstart<span class="pl-pds">&quot;</span></span><span class="pl-k">in</span> <span class="pl-c1">document</span>.<span class="pl-c1">createElement</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>div<span class="pl-pds">&quot;</span></span>);<span class="pl-k">return</span> a<span class="pl-k">||</span>b<span class="pl-k">?</span><span class="pl-k">!</span><span class="pl-c1">0</span><span class="pl-k">:</span><span class="pl-k">!</span><span class="pl-c1">1</span>};<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span><span class="pl-k">=</span>{};<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mozilla</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">opera</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">safari</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">chrome</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">androidStock</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">msie</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">edge</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">hasTouch</span><span class="pl-k">=</span><span class="pl-en">isTouchSupported</span>();<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">ua</span><span class="pl-k">=</span>nAgt;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-c1">navigator</span>.<span class="pl-c1">appName</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span><span class="pl-k">+</span></td>
      </tr>
      <tr>
        <td id="L1016" class="blob-num js-line-number" data-line-number="1016"></td>
        <td id="LC1016" class="blob-code blob-code-inner js-file-line">		<span class="pl-c1">parseFloat</span>(<span class="pl-c1">navigator</span>.<span class="pl-c1">appVersion</span>);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">majorVersion</span><span class="pl-k">=</span><span class="pl-c1">parseInt</span>(<span class="pl-c1">navigator</span>.<span class="pl-c1">appVersion</span>,<span class="pl-c1">10</span>);<span class="pl-k">var</span> nameOffset,verOffset,ix;<span class="pl-k">if</span>(<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Opera<span class="pl-pds">&quot;</span></span>)))<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">opera</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Opera<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">6</span>),<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Version<span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">8</span>));<span class="pl-k">else</span> <span class="pl-k">if</span>(<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>OPR<span class="pl-pds">&quot;</span></span>)))<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">opera</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Opera<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span></td>
      </tr>
      <tr>
        <td id="L1017" class="blob-num js-line-number" data-line-number="1017"></td>
        <td id="LC1017" class="blob-code blob-code-inner js-file-line">		<span class="pl-c1">4</span>);<span class="pl-k">else</span> <span class="pl-k">if</span>(<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>MSIE<span class="pl-pds">&quot;</span></span>)))<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">msie</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Microsoft Internet Explorer<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">5</span>);<span class="pl-k">else</span> <span class="pl-k">if</span>(<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Trident<span class="pl-pds">&quot;</span></span>)){<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">msie</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Microsoft Internet Explorer<span class="pl-pds">&quot;</span></span>;<span class="pl-k">var</span> start<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>rv:<span class="pl-pds">&quot;</span></span>)<span class="pl-k">+</span><span class="pl-c1">3</span>,end<span class="pl-k">=</span>start<span class="pl-k">+</span><span class="pl-c1">4</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(start,end)}<span class="pl-k">else</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Edge<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">edge</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Microsoft Edge<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span></td>
      </tr>
      <tr>
        <td id="L1018" class="blob-num js-line-number" data-line-number="1018"></td>
        <td id="LC1018" class="blob-code blob-code-inner js-file-line">		<span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">5</span>))<span class="pl-k">:</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Chrome<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">chrome</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Chrome<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">7</span>))<span class="pl-k">:</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">&lt;</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>mozilla/5.0<span class="pl-pds">&quot;</span></span>)<span class="pl-k">&amp;&amp;</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">&lt;</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>android <span class="pl-pds">&quot;</span></span>)<span class="pl-k">&amp;&amp;</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">&lt;</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>applewebkit<span class="pl-pds">&quot;</span></span>)<span class="pl-k">&amp;&amp;!</span>(<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">&lt;</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>chrome<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Chrome<span class="pl-pds">&quot;</span></span>),<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">androidStock</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>androidStock<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span></td>
      </tr>
      <tr>
        <td id="L1019" class="blob-num js-line-number" data-line-number="1019"></td>
        <td id="LC1019" class="blob-code blob-code-inner js-file-line">		<span class="pl-c1">7</span>))<span class="pl-k">:</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Safari<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">safari</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Safari<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">7</span>),<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Version<span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">8</span>)))<span class="pl-k">:</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>AppleWebkit<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">webkit</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">safari</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Safari<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">7</span>),<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Version<span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span></td>
      </tr>
      <tr>
        <td id="L1020" class="blob-num js-line-number" data-line-number="1020"></td>
        <td id="LC1020" class="blob-code blob-code-inner js-file-line">		<span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">8</span>)))<span class="pl-k">:</span><span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>Firefox<span class="pl-pds">&quot;</span></span>))<span class="pl-k">?</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mozilla</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>Firefox<span class="pl-pds">&quot;</span></span>,<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">8</span>))<span class="pl-k">:</span>(nameOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">lastIndexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span> <span class="pl-pds">&quot;</span></span>)<span class="pl-k">+</span><span class="pl-c1">1</span>)<span class="pl-k">&lt;</span>(verOffset<span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">lastIndexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>/<span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(nameOffset,verOffset),<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">nAgt</span>.<span class="pl-c1">substring</span>(verOffset<span class="pl-k">+</span><span class="pl-c1">1</span>),<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span>.<span class="pl-c1">toLowerCase</span>()<span class="pl-k">==</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span>.<span class="pl-c1">toUpperCase</span>()<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">name</span><span class="pl-k">=</span><span class="pl-c1">navigator</span>.<span class="pl-c1">appName</span>));<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(ix<span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>;<span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span></td>
      </tr>
      <tr>
        <td id="L1021" class="blob-num js-line-number" data-line-number="1021"></td>
        <td id="LC1021" class="blob-code blob-code-inner js-file-line">(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span>.<span class="pl-c1">substring</span>(<span class="pl-c1">0</span>,ix));<span class="pl-k">-</span><span class="pl-c1">1</span><span class="pl-k">!=</span>(ix<span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span>.<span class="pl-c1">indexOf</span>(<span class="pl-s"><span class="pl-pds">&quot;</span> <span class="pl-pds">&quot;</span></span>))<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span>.<span class="pl-c1">substring</span>(<span class="pl-c1">0</span>,ix));<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">majorVersion</span><span class="pl-k">=</span><span class="pl-c1">parseInt</span>(<span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span><span class="pl-k">+</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span>,<span class="pl-c1">10</span>);<span class="pl-c1">isNaN</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">majorVersion</span>)<span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">fullVersion</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span><span class="pl-pds">&quot;</span></span><span class="pl-k">+</span><span class="pl-c1">parseFloat</span>(<span class="pl-c1">navigator</span>.<span class="pl-c1">appVersion</span>),<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">majorVersion</span><span class="pl-k">=</span><span class="pl-c1">parseInt</span>(<span class="pl-c1">navigator</span>.<span class="pl-c1">appVersion</span>,<span class="pl-c1">10</span>));<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-c1">version</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">majorVersion</span>}</td>
      </tr>
      <tr>
        <td id="L1022" class="blob-num js-line-number" data-line-number="1022"></td>
        <td id="LC1022" class="blob-code blob-code-inner js-file-line"><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">android</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>Android<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">blackberry</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>BlackBerry<span class="pl-k">|</span>BB<span class="pl-k">|</span>PlayBook<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">ios</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>iPhone<span class="pl-k">|</span>iPad<span class="pl-k">|</span>iPod<span class="pl-k">|</span>webOS<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">operaMobile</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>Opera Mini<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">windowsMobile</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>IEMobile<span class="pl-k">|</span>Windows Phone<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">kindle</span><span class="pl-k">=</span><span class="pl-sr"><span class="pl-pds">/</span>Kindle<span class="pl-k">|</span>Silk<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);<span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">android</span><span class="pl-k">||</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">blackberry</span><span class="pl-k">||</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">ios</span><span class="pl-k">||</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">windowsMobile</span><span class="pl-k">||</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">operaMobile</span><span class="pl-k">||</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">kindle</span>;</td>
      </tr>
      <tr>
        <td id="L1023" class="blob-num js-line-number" data-line-number="1023"></td>
        <td id="LC1023" class="blob-code blob-code-inner js-file-line"><span class="pl-smi">jQuery</span>.<span class="pl-smi">isMobile</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span>;<span class="pl-smi">jQuery</span>.<span class="pl-smi">isTablet</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">&amp;&amp;</span><span class="pl-c1">765</span><span class="pl-k">&lt;</span><span class="pl-en">jQuery</span>(<span class="pl-c1">window</span>).<span class="pl-c1">width</span>();<span class="pl-smi">jQuery</span>.<span class="pl-smi">isAndroidDefault</span><span class="pl-k">=</span><span class="pl-smi">jQuery</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">android</span><span class="pl-k">&amp;&amp;!</span><span class="pl-sr"><span class="pl-pds">/</span>chrome<span class="pl-pds">/</span>i</span>.<span class="pl-c1">test</span>(nAgt);</td>
      </tr>
      <tr>
        <td id="L1024" class="blob-num js-line-number" data-line-number="1024"></td>
        <td id="LC1024" class="blob-code blob-code-inner js-file-line">;<span class="pl-c">/*___________________________________________________________________________________________________________________________________________________</span></td>
      </tr>
      <tr>
        <td id="L1025" class="blob-num js-line-number" data-line-number="1025"></td>
        <td id="LC1025" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ jquery.mb.components                                                                                                                             _</span></td>
      </tr>
      <tr>
        <td id="L1026" class="blob-num js-line-number" data-line-number="1026"></td>
        <td id="LC1026" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1027" class="blob-num js-line-number" data-line-number="1027"></td>
        <td id="LC1027" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ file: jquery.mb.simpleSlider.min.js                                                                                                              _</span></td>
      </tr>
      <tr>
        <td id="L1028" class="blob-num js-line-number" data-line-number="1028"></td>
        <td id="LC1028" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ last modified: 16/05/15 23.45                                                                                                                    _</span></td>
      </tr>
      <tr>
        <td id="L1029" class="blob-num js-line-number" data-line-number="1029"></td>
        <td id="LC1029" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1030" class="blob-num js-line-number" data-line-number="1030"></td>
        <td id="LC1030" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Open Lab s.r.l., Florence - Italy                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L1031" class="blob-num js-line-number" data-line-number="1031"></td>
        <td id="LC1031" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1032" class="blob-num js-line-number" data-line-number="1032"></td>
        <td id="LC1032" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ email: matteo@open-lab.com                                                                                                                       _</span></td>
      </tr>
      <tr>
        <td id="L1033" class="blob-num js-line-number" data-line-number="1033"></td>
        <td id="LC1033" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ site: http://pupunzi.com                                                                                                                         _</span></td>
      </tr>
      <tr>
        <td id="L1034" class="blob-num js-line-number" data-line-number="1034"></td>
        <td id="LC1034" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _       http://open-lab.com                                                                                                                        _</span></td>
      </tr>
      <tr>
        <td id="L1035" class="blob-num js-line-number" data-line-number="1035"></td>
        <td id="LC1035" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ blog: http://pupunzi.open-lab.com                                                                                                                _</span></td>
      </tr>
      <tr>
        <td id="L1036" class="blob-num js-line-number" data-line-number="1036"></td>
        <td id="LC1036" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Q&amp;A:  http://jquery.pupunzi.com                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1037" class="blob-num js-line-number" data-line-number="1037"></td>
        <td id="LC1037" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1038" class="blob-num js-line-number" data-line-number="1038"></td>
        <td id="LC1038" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Licences: MIT, GPL                                                                                                                               _</span></td>
      </tr>
      <tr>
        <td id="L1039" class="blob-num js-line-number" data-line-number="1039"></td>
        <td id="LC1039" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.opensource.org/licenses/mit-license.php                                                                                            _</span></td>
      </tr>
      <tr>
        <td id="L1040" class="blob-num js-line-number" data-line-number="1040"></td>
        <td id="LC1040" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _    http://www.gnu.org/licenses/gpl.html                                                                                                          _</span></td>
      </tr>
      <tr>
        <td id="L1041" class="blob-num js-line-number" data-line-number="1041"></td>
        <td id="LC1041" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _                                                                                                                                                  _</span></td>
      </tr>
      <tr>
        <td id="L1042" class="blob-num js-line-number" data-line-number="1042"></td>
        <td id="LC1042" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> _ Copyright (c) 2001-2015. Matteo Bicocchi (Pupunzi);                                                                                              _</span></td>
      </tr>
      <tr>
        <td id="L1043" class="blob-num js-line-number" data-line-number="1043"></td>
        <td id="LC1043" class="blob-code blob-code-inner js-file-line"><span class="pl-c"> ___________________________________________________________________________________________________________________________________________________*/</span></td>
      </tr>
      <tr>
        <td id="L1044" class="blob-num js-line-number" data-line-number="1044"></td>
        <td id="LC1044" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1045" class="blob-num js-line-number" data-line-number="1045"></td>
        <td id="LC1045" class="blob-code blob-code-inner js-file-line">var nAgt=navigator.userAgent;if(!jQuery.browser){jQuery.browser={},jQuery.browser.mozilla=!1,jQuery.browser.webkit=!1,jQuery.browser.opera=!1,jQuery.browser.safari=!1,jQuery.browser.chrome=!1,jQuery.browser.androidStock=!1,jQuery.browser.msie=!1,jQuery.browser.ua=nAgt,jQuery.browser.name=navigator.appName,jQuery.browser.fullVersion=&quot;&quot;+parseFloat(navigator.appVersion),jQuery.browser.majorVersion=parseInt(navigator.appVersion,10);var nameOffset,verOffset,ix;if(-1!=(verOffset=nAgt.indexOf(&quot;Opera&quot;)))jQuery.browser.opera=!0,jQuery.browser.name=&quot;Opera&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+6),-1!=(verOffset=nAgt.indexOf(&quot;Version&quot;))&amp;&amp;(jQuery.browser.fullVersion=nAgt.substring(verOffset+8));else if(-1!=(verOffset=nAgt.indexOf(&quot;OPR&quot;)))jQuery.browser.opera=!0,jQuery.browser.name=&quot;Opera&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+4);else if(-1!=(verOffset=nAgt.indexOf(&quot;MSIE&quot;)))jQuery.browser.msie=!0,jQuery.browser.name=&quot;Microsoft Internet Explorer&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+5);else if(-1!=nAgt.indexOf(&quot;Trident&quot;)||-1!=nAgt.indexOf(&quot;Edge&quot;)){jQuery.browser.msie=!0,jQuery.browser.name=&quot;Microsoft Internet Explorer&quot;;var start=nAgt.indexOf(&quot;rv:&quot;)+3,end=start+4;jQuery.browser.fullVersion=nAgt.substring(start,end)}else-1!=(verOffset=nAgt.indexOf(&quot;Chrome&quot;))?(jQuery.browser.webkit=!0,jQuery.browser.chrome=!0,jQuery.browser.name=&quot;Chrome&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+7)):nAgt.indexOf(&quot;mozilla/5.0&quot;)&gt;-1&amp;&amp;nAgt.indexOf(&quot;android &quot;)&gt;-1&amp;&amp;nAgt.indexOf(&quot;applewebkit&quot;)&gt;-1&amp;&amp;!(nAgt.indexOf(&quot;chrome&quot;)&gt;-1)?(verOffset=nAgt.indexOf(&quot;Chrome&quot;),jQuery.browser.webkit=!0,jQuery.browser.androidStock=!0,jQuery.browser.name=&quot;androidStock&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+7)):-1!=(verOffset=nAgt.indexOf(&quot;Safari&quot;))?(jQuery.browser.webkit=!0,jQuery.browser.safari=!0,jQuery.browser.name=&quot;Safari&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+7),-1!=(verOffset=nAgt.indexOf(&quot;Version&quot;))&amp;&amp;(jQuery.browser.fullVersion=nAgt.substring(verOffset+8))):-1!=(verOffset=nAgt.indexOf(&quot;AppleWebkit&quot;))?(jQuery.browser.webkit=!0,jQuery.browser.safari=!0,jQuery.browser.name=&quot;Safari&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+7),-1!=(verOffset=nAgt.indexOf(&quot;Version&quot;))&amp;&amp;(jQuery.browser.fullVersion=nAgt.substring(verOffset+8))):-1!=(verOffset=nAgt.indexOf(&quot;Firefox&quot;))?(jQuery.browser.mozilla=!0,jQuery.browser.name=&quot;Firefox&quot;,jQuery.browser.fullVersion=nAgt.substring(verOffset+8)):(nameOffset=nAgt.lastIndexOf(&quot; &quot;)+1)&lt;(verOffset=nAgt.lastIndexOf(&quot;/&quot;))&amp;&amp;(jQuery.browser.name=nAgt.substring(nameOffset,verOffset),jQuery.browser.fullVersion=nAgt.substring(verOffset+1),jQuery.browser.name.toLowerCase()==jQuery.browser.name.toUpperCase()&amp;&amp;(jQuery.browser.name=navigator.appName));-1!=(ix=jQuery.browser.fullVersion.indexOf(&quot;;&quot;))&amp;&amp;(jQuery.browser.fullVersion=jQuery.browser.fullVersion.substring(0,ix)),-1!=(ix=jQuery.browser.fullVersion.indexOf(&quot; &quot;))&amp;&amp;(jQuery.browser.fullVersion=jQuery.browser.fullVersion.substring(0,ix)),jQuery.browser.majorVersion=parseInt(&quot;&quot;+jQuery.browser.fullVersion,10),isNaN(jQuery.browser.majorVersion)&amp;&amp;(jQuery.browser.fullVersion=&quot;&quot;+parseFloat(navigator.appVersion),jQuery.browser.majorVersion=parseInt(navigator.appVersion,10)),jQuery.browser.version=jQuery.browser.majorVersion}jQuery.browser.android=/Android/i.test(nAgt),jQuery.browser.blackberry=/BlackBerry|BB|PlayBook/i.test(nAgt),jQuery.browser.ios=/iPhone|iPad|iPod|webOS/i.test(nAgt),jQuery.browser.operaMobile=/Opera Mini/i.test(nAgt),jQuery.browser.windowsMobile=/IEMobile|Windows Phone/i.test(nAgt),jQuery.browser.kindle=/Kindle|Silk/i.test(nAgt),jQuery.browser.mobile=jQuery.browser.android||jQuery.browser.blackberry||jQuery.browser.ios||jQuery.browser.windowsMobile||jQuery.browser.operaMobile||jQuery.browser.kindle,jQuery.isMobile=jQuery.browser.mobile,jQuery.isTablet=jQuery.browser.mobile&amp;&amp;jQuery(window).width()&gt;765,jQuery.isAndroidDefault=jQuery.browser.android&amp;&amp;!/chrome/i.test(nAgt);</td>
      </tr>
      <tr>
        <td id="L1046" class="blob-num js-line-number" data-line-number="1046"></td>
        <td id="LC1046" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1047" class="blob-num js-line-number" data-line-number="1047"></td>
        <td id="LC1047" class="blob-code blob-code-inner js-file-line">(<span class="pl-k">function</span>(<span class="pl-smi">b</span>){<span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span><span class="pl-k">=</span>{defaults<span class="pl-k">:</span>{initialval<span class="pl-k">:</span><span class="pl-c1">0</span>,scale<span class="pl-k">:</span><span class="pl-c1">100</span>,orientation<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span>,readonly<span class="pl-k">:</span><span class="pl-k">!</span><span class="pl-c1">1</span>,callback<span class="pl-k">:</span><span class="pl-k">!</span><span class="pl-c1">1</span>},events<span class="pl-k">:</span>{start<span class="pl-k">:</span><span class="pl-smi">b</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">?</span><span class="pl-s"><span class="pl-pds">&quot;</span>touchstart<span class="pl-pds">&quot;</span></span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>mousedown<span class="pl-pds">&quot;</span></span>,end<span class="pl-k">:</span><span class="pl-smi">b</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">?</span><span class="pl-s"><span class="pl-pds">&quot;</span>touchend<span class="pl-pds">&quot;</span></span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>mouseup<span class="pl-pds">&quot;</span></span>,move<span class="pl-k">:</span><span class="pl-smi">b</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">?</span><span class="pl-s"><span class="pl-pds">&quot;</span>touchmove<span class="pl-pds">&quot;</span></span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>mousemove<span class="pl-pds">&quot;</span></span>},<span class="pl-en">init</span><span class="pl-k">:</span><span class="pl-k">function</span>(<span class="pl-smi">c</span>){<span class="pl-k">return</span> <span class="pl-c1">this</span>.<span class="pl-en">each</span>(<span class="pl-k">function</span>(){<span class="pl-k">var</span> a<span class="pl-k">=</span><span class="pl-c1">this</span>,d<span class="pl-k">=</span><span class="pl-en">b</span>(a);<span class="pl-smi">d</span>.<span class="pl-en">addClass</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>simpleSlider<span class="pl-pds">&quot;</span></span>);<span class="pl-smi">a</span>.<span class="pl-smi">opt</span><span class="pl-k">=</span>{};<span class="pl-smi">b</span>.<span class="pl-en">extend</span>(<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>,<span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">defaults</span>,c);<span class="pl-smi">b</span>.<span class="pl-en">extend</span>(<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>,<span class="pl-smi">d</span>.<span class="pl-c1">data</span>());<span class="pl-k">var</span> e<span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-s"><span class="pl-pds">&quot;</span>horizontal<span class="pl-pds">&quot;</span></span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>vertical<span class="pl-pds">&quot;</span></span>,e<span class="pl-k">=</span><span class="pl-en">b</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>&lt;div/&gt;<span class="pl-pds">&quot;</span></span>).<span class="pl-en">addClass</span>(<span class="pl-s"><span class="pl-pds">&quot;</span>level<span class="pl-pds">&quot;</span></span>).<span class="pl-en">addClass</span>(e);</td>
      </tr>
      <tr>
        <td id="L1048" class="blob-num js-line-number" data-line-number="1048"></td>
        <td id="LC1048" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">d</span>.<span class="pl-c1">prepend</span>(e);<span class="pl-smi">a</span>.<span class="pl-smi">level</span><span class="pl-k">=</span>e;<span class="pl-smi">d</span>.<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>default<span class="pl-pds">&quot;</span></span>});<span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">&amp;&amp;</span>(<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">=</span><span class="pl-en">b</span>(a).<span class="pl-c1">outerWidth</span>());<span class="pl-smi">d</span>.<span class="pl-en">updateSliderVal</span>();<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">readonly</span><span class="pl-k">||</span>(<span class="pl-smi">d</span>.<span class="pl-en">on</span>(<span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">events</span>.<span class="pl-c1">start</span>,<span class="pl-k">function</span>(<span class="pl-smi">c</span>){<span class="pl-smi">b</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">&amp;&amp;</span>(c<span class="pl-k">=</span><span class="pl-smi">c</span>.<span class="pl-smi">changedTouches</span>[<span class="pl-c1">0</span>]);<span class="pl-smi">a</span>.<span class="pl-smi">canSlide</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">0</span>;<span class="pl-smi">d</span>.<span class="pl-en">updateSliderVal</span>(c);<span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-smi">d</span>.<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>col-resize<span class="pl-pds">&quot;</span></span>})<span class="pl-k">:</span><span class="pl-smi">d</span>.<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>row-resize<span class="pl-pds">&quot;</span></span>});<span class="pl-smi">c</span>.<span class="pl-c1">preventDefault</span>();<span class="pl-smi">c</span>.<span class="pl-c1">stopPropagation</span>()}),<span class="pl-en">b</span>(<span class="pl-c1">document</span>).<span class="pl-en">on</span>(<span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">events</span>.<span class="pl-smi">move</span>,<span class="pl-k">function</span>(<span class="pl-smi">c</span>){<span class="pl-smi">b</span>.<span class="pl-smi">browser</span>.<span class="pl-smi">mobile</span><span class="pl-k">&amp;&amp;</span>(c<span class="pl-k">=</span><span class="pl-smi">c</span>.<span class="pl-smi">changedTouches</span>[<span class="pl-c1">0</span>]);<span class="pl-smi">a</span>.<span class="pl-smi">canSlide</span><span class="pl-k">&amp;&amp;</span>(<span class="pl-en">b</span>(<span class="pl-c1">document</span>).<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>default<span class="pl-pds">&quot;</span></span>}),</td>
      </tr>
      <tr>
        <td id="L1049" class="blob-num js-line-number" data-line-number="1049"></td>
        <td id="LC1049" class="blob-code blob-code-inner js-file-line">			<span class="pl-smi">d</span>.<span class="pl-en">updateSliderVal</span>(c),<span class="pl-smi">c</span>.<span class="pl-c1">preventDefault</span>(),<span class="pl-smi">c</span>.<span class="pl-c1">stopPropagation</span>())}).<span class="pl-en">on</span>(<span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">events</span>.<span class="pl-smi">end</span>,<span class="pl-k">function</span>(){<span class="pl-en">b</span>(<span class="pl-c1">document</span>).<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span>});<span class="pl-smi">a</span>.<span class="pl-smi">canSlide</span><span class="pl-k">=</span><span class="pl-k">!</span><span class="pl-c1">1</span>;<span class="pl-smi">d</span>.<span class="pl-en">css</span>({cursor<span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>auto<span class="pl-pds">&quot;</span></span>})}))})},<span class="pl-en">updateSliderVal</span><span class="pl-k">:</span><span class="pl-k">function</span>(<span class="pl-smi">c</span>){<span class="pl-k">var</span> a<span class="pl-k">=</span><span class="pl-c1">this</span>.<span class="pl-c1">get</span>(<span class="pl-c1">0</span>);<span class="pl-k">if</span>(<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>){<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>number<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> <span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-en">initialval</span>(a);<span class="pl-k">var</span> d<span class="pl-k">=</span><span class="pl-en">b</span>(a).<span class="pl-c1">outerWidth</span>(),e<span class="pl-k">=</span><span class="pl-en">b</span>(a).<span class="pl-c1">outerHeight</span>();<span class="pl-smi">a</span>.<span class="pl-c1">x</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>object<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> c<span class="pl-k">?</span><span class="pl-smi">c</span>.<span class="pl-smi">clientX</span><span class="pl-k">+</span><span class="pl-c1">document</span>.<span class="pl-c1">body</span>.<span class="pl-smi">scrollLeft</span><span class="pl-k">-</span><span class="pl-c1">this</span>.<span class="pl-en">offset</span>().<span class="pl-c1">left</span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>number<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> c<span class="pl-k">?</span>c<span class="pl-k">*</span>d<span class="pl-k">/</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">*</span>d<span class="pl-k">/</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span>;</td>
      </tr>
      <tr>
        <td id="L1050" class="blob-num js-line-number" data-line-number="1050"></td>
        <td id="LC1050" class="blob-code blob-code-inner js-file-line">	<span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>object<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> c<span class="pl-k">?</span><span class="pl-smi">c</span>.<span class="pl-smi">clientY</span><span class="pl-k">+</span><span class="pl-c1">document</span>.<span class="pl-c1">body</span>.<span class="pl-smi">scrollTop</span><span class="pl-k">-</span><span class="pl-c1">this</span>.<span class="pl-en">offset</span>().<span class="pl-c1">top</span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>number<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> c<span class="pl-k">?</span>(<span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">-</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">-</span>c)<span class="pl-k">*</span>e<span class="pl-k">/</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">initialval</span><span class="pl-k">*</span>e<span class="pl-k">/</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span>;<span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">=</span><span class="pl-c1">this</span>.<span class="pl-c1">outerHeight</span>()<span class="pl-k">-</span><span class="pl-smi">a</span>.<span class="pl-c1">y</span>;<span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">=</span><span class="pl-smi">a</span>.<span class="pl-c1">x</span><span class="pl-k">*</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">/</span>d;<span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">=</span><span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">*</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">/</span>e;<span class="pl-smi">a</span>.<span class="pl-smi">outOfRangeX</span><span class="pl-k">=</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">&gt;</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">-</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-c1">0</span><span class="pl-k">&gt;</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">:</span><span class="pl-c1">0</span>;<span class="pl-smi">a</span>.<span class="pl-smi">outOfRangeY</span><span class="pl-k">=</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">&gt;</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">-</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-c1">0</span><span class="pl-k">&gt;</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">:</span><span class="pl-c1">0</span>;<span class="pl-smi">a</span>.<span class="pl-smi">outOfRange</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">outOfRangeX</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">outOfRangeY</span>;<span class="pl-smi">a</span>.<span class="pl-c1">value</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">&quot;</span>undefined<span class="pl-pds">&quot;</span></span><span class="pl-k">!=</span><span class="pl-k">typeof</span> c<span class="pl-k">?</span></td>
      </tr>
      <tr>
        <td id="L1051" class="blob-num js-line-number" data-line-number="1051"></td>
        <td id="LC1051" class="blob-code blob-code-inner js-file-line">					<span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-c1">x</span><span class="pl-k">&gt;=</span><span class="pl-c1">this</span>.<span class="pl-c1">outerWidth</span>()<span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-c1">0</span><span class="pl-k">&gt;=</span><span class="pl-smi">a</span>.<span class="pl-c1">x</span><span class="pl-k">?</span><span class="pl-c1">0</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">&gt;=</span><span class="pl-c1">this</span>.<span class="pl-c1">outerHeight</span>()<span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">scale</span><span class="pl-k">:</span><span class="pl-c1">0</span><span class="pl-k">&gt;=</span><span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">?</span><span class="pl-c1">0</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span><span class="pl-k">:</span><span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleX</span><span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">scaleY</span>;<span class="pl-s"><span class="pl-pds">&quot;</span>h<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">orientation</span><span class="pl-k">?</span><span class="pl-smi">a</span>.<span class="pl-smi">level</span>.<span class="pl-c1">width</span>(<span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>(<span class="pl-c1">100</span><span class="pl-k">*</span><span class="pl-smi">a</span>.<span class="pl-c1">x</span><span class="pl-k">/</span>d)<span class="pl-k">+</span><span class="pl-s"><span class="pl-pds">&quot;</span>%<span class="pl-pds">&quot;</span></span>)<span class="pl-k">:</span><span class="pl-smi">a</span>.<span class="pl-smi">level</span>.<span class="pl-c1">height</span>(<span class="pl-c1">Math</span>.<span class="pl-c1">floor</span>(<span class="pl-c1">100</span><span class="pl-k">*</span><span class="pl-smi">a</span>.<span class="pl-c1">y</span><span class="pl-k">/</span>e));<span class="pl-s"><span class="pl-pds">&quot;</span>function<span class="pl-pds">&quot;</span></span><span class="pl-k">==</span><span class="pl-k">typeof</span> <span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-smi">callback</span><span class="pl-k">&amp;&amp;</span><span class="pl-smi">a</span>.<span class="pl-smi">opt</span>.<span class="pl-en">callback</span>(a)}}};<span class="pl-smi">b</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">simpleSlider</span><span class="pl-k">=</span><span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">init</span>;<span class="pl-smi">b</span>.<span class="pl-smi">fn</span>.<span class="pl-smi">updateSliderVal</span><span class="pl-k">=</span><span class="pl-smi">b</span>.<span class="pl-smi">simpleSlider</span>.<span class="pl-smi">updateSliderVal</span>})(jQuery);</td>
      </tr>
</table>

  </div>

</div>

<button type="button" data-facebox="#jump-to-line" data-facebox-class="linejump" data-hotkey="l" class="d-none">Jump to Line</button>
<div id="jump-to-line" style="display:none">
  <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="" class="js-jump-to-line-form" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
    <input class="form-control linejump-input js-jump-to-line-field" type="text" placeholder="Jump to line&hellip;" aria-label="Jump to line" autofocus>
    <button type="submit" class="btn">Go</button>
</form></div>


  </div>
  <div class="modal-backdrop js-touch-events"></div>
</div>

    </div>
  </div>

  </div>

      <div class="container site-footer-container">
  <div class="site-footer" role="contentinfo">
    <ul class="site-footer-links float-right">
        <li><a href="https://github.com/contact" data-ga-click="Footer, go to contact, text:contact">Contact GitHub</a></li>
      <li><a href="https://developer.github.com" data-ga-click="Footer, go to api, text:api">API</a></li>
      <li><a href="https://training.github.com" data-ga-click="Footer, go to training, text:training">Training</a></li>
      <li><a href="https://shop.github.com" data-ga-click="Footer, go to shop, text:shop">Shop</a></li>
        <li><a href="https://github.com/blog" data-ga-click="Footer, go to blog, text:blog">Blog</a></li>
        <li><a href="https://github.com/about" data-ga-click="Footer, go to about, text:about">About</a></li>

    </ul>

    <a href="https://github.com" aria-label="Homepage" class="site-footer-mark" title="GitHub">
      <svg aria-hidden="true" class="octicon octicon-mark-github" height="24" version="1.1" viewBox="0 0 16 16" width="24"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0 0 16 8c0-4.42-3.58-8-8-8z"/></svg>
</a>
    <ul class="site-footer-links">
      <li>&copy; 2017 <span title="0.50484s from github-fe126-cp1-prd.iad.github.net">GitHub</span>, Inc.</li>
        <li><a href="https://github.com/site/terms" data-ga-click="Footer, go to terms, text:terms">Terms</a></li>
        <li><a href="https://github.com/site/privacy" data-ga-click="Footer, go to privacy, text:privacy">Privacy</a></li>
        <li><a href="https://github.com/security" data-ga-click="Footer, go to security, text:security">Security</a></li>
        <li><a href="https://status.github.com/" data-ga-click="Footer, go to status, text:status">Status</a></li>
        <li><a href="https://help.github.com" data-ga-click="Footer, go to help, text:help">Help</a></li>
    </ul>
  </div>
</div>



  

  <div id="ajax-error-message" class="ajax-error-message flash flash-error">
    <svg aria-hidden="true" class="octicon octicon-alert" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.865 1.52c-.18-.31-.51-.5-.87-.5s-.69.19-.87.5L.275 13.5c-.18.31-.18.69 0 1 .19.31.52.5.87.5h13.7c.36 0 .69-.19.86-.5.17-.31.18-.69.01-1L8.865 1.52zM8.995 13h-2v-2h2v2zm0-3h-2V6h2v4z"/></svg>
    <button type="button" class="flash-close js-flash-close js-ajax-error-dismiss" aria-label="Dismiss error">
      <svg aria-hidden="true" class="octicon octicon-x" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
    </button>
    You can't perform that action at this time.
  </div>


    <script crossorigin="anonymous" integrity="sha256-ikMY/+oJoM24IUt2zykmufagztMYoxe+1BnbGSFMaQ0=" src="https://assets-cdn.github.com/assets/compat-8a4318ffea09a0cdb8214b76cf2926b9f6a0ced318a317bed419db19214c690d.js"></script>
    <script crossorigin="anonymous" integrity="sha256-bRCeda2EcbpBUIJybADDX7kpzquXUIJJKDXxHsqMB9k=" src="https://assets-cdn.github.com/assets/frameworks-6d109e75ad8471ba415082726c00c35fb929ceab975082492835f11eca8c07d9.js"></script>
    <script async="async" crossorigin="anonymous" integrity="sha256-qGHbgGiTjt6U7WfTYqxLOUZH3MSkDsEBv1D99i5rptc=" src="https://assets-cdn.github.com/assets/github-a861db8068938ede94ed67d362ac4b394647dcc4a40ec101bf50fdf62e6ba6d7.js"></script>
    
    
    
    
  <div class="js-stale-session-flash stale-session-flash flash flash-warn flash-banner d-none">
    <svg aria-hidden="true" class="octicon octicon-alert" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.865 1.52c-.18-.31-.51-.5-.87-.5s-.69.19-.87.5L.275 13.5c-.18.31-.18.69 0 1 .19.31.52.5.87.5h13.7c.36 0 .69-.19.86-.5.17-.31.18-.69.01-1L8.865 1.52zM8.995 13h-2v-2h2v2zm0-3h-2V6h2v4z"/></svg>
    <span class="signed-in-tab-flash">You signed in with another tab or window. <a href="">Reload</a> to refresh your session.</span>
    <span class="signed-out-tab-flash">You signed out in another tab or window. <a href="">Reload</a> to refresh your session.</span>
  </div>
  <div class="facebox" id="facebox" style="display:none;">
  <div class="facebox-popup">
    <div class="facebox-content" role="dialog" aria-labelledby="facebox-header" aria-describedby="facebox-description">
    </div>
    <button type="button" class="facebox-close js-facebox-close" aria-label="Close modal">
      <svg aria-hidden="true" class="octicon octicon-x" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
    </button>
  </div>
</div>


  </body>
</html>

