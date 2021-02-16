# Fading World

Unity 2020.2.4f1

Reference :[https://thoughtbot.com/blog/how-to-git-with-unity](https://thoughtbot.com/blog/how-to-git-with-unity)

Basic Git usage: [https://guides.github.com/activities/hello-world/](https://guides.github.com/activities/hello-world/#pr)

### Install Git LFS

Simply download and install Git LFS from [https://git-lfs.github.com/](https://git-lfs.github.com/)

The .gitattributes file has already been configured to use LFS for the most common large file types

### Adding stuff: using git-flow paradigm

To ensure smooth working and repo management, please follow the git-flow branching method when adding new stuff to the project.

- **develop** branch will contain work-in-progress features & deliverables
- **master** branch will track the current version of the final product

If you have made small changes, feel free to make a commit directly on the **develop** branch.

When you start to develop a new feature/deliverable, you should:

1. Create a new branch from develop, using the naming convention **feature/name-of-your-feature**
2. Checkout to that branch while you work on that specific feature
3. Once the feature is complete, **merge** the feature branch in develop (IMPORTANT: remember to comment commits adequately so everyone knows what's going on!
4. We'll see whether it is appropriate to make pull requests or let people merge directly into develop as the project goes on.

Once a bunch of features are done and the master branch can be updated, then it is time to make a **release**. The process is the same as the feature branch, but at the end the release branch should be merged into **master**. Releases should be decided as a group and preferably be a single person's responsibility.

For any questions, ask Andrea.
