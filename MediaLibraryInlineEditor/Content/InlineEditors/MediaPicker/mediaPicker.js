'use strict';

class mediaPicker {
    constructor () {

        this.setBinds = this.setBinds.bind(this);
        this.setBinds();
    }

    init (foldersURL, filesURL) {
        this.foldersURL = foldersURL;
        this.filesURL = filesURL;
        this.storage = this.getObjectFromLocalStorage();
        this.isCreated = false;
        if (this.storage === null) {
            this.storage = {
                hash: ''
            }
        }
        this.checkOptionsFromLocalStorage();
        this.setTemplate();
        this.createDom();
        this.addListeners();
    }

    setBinds () {
        this.init = this.init.bind(this);
        this.setTemplate = this.setTemplate.bind(this);
        this.changePreviewList = this.changePreviewList.bind(this);
        this.closeLastPreview = this.closeLastPreview.bind(this);
        this.showNextPreview = this.showNextPreview.bind(this);
        this.changeLastSelected = this.changeLastSelected.bind(this);
        this.checkStorage = this.checkStorage.bind(this);
        this.updateLocalStorage = this.updateLocalStorage.bind(this);
        this.getObjectFromLocalStorage = this.getObjectFromLocalStorage.bind(this);
        this.startDraw = this.startDraw.bind(this);
        this.createDom = this.createDom.bind(this);
        this.addListeners = this.addListeners.bind(this);
        this.openPicker = this.openPicker.bind(this);
        this.showModal = this.showModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.setFocusItem = this.setFocusItem.bind(this);
        this.makeSingleEvent = this.makeSingleEvent.bind(this);
        this.cleanPicker = this.cleanPicker.bind(this);
        this.onConfirm = this.onConfirm.bind(this);
        this.onEnterPress = this.onEnterPress.bind(this);
        this.onModalZoneClick = this.onModalZoneClick.bind(this);
        this.onClearClick = this.onClearClick.bind(this);
        this.makeSingleNullEvent = this.makeSingleNullEvent.bind(this);
        this.onEscPress = this.onEscPress.bind(this);
        this.setOptions = this.setOptions.bind(this);
        this.makeInManyEvent = this.makeInManyEvent.bind(this);
        this.dispatchEvent = this.dispatchEvent.bind(this);
        this.setDynamicStyles = this.setDynamicStyles.bind(this);
        this.checkOptionsFromLocalStorage = this.checkOptionsFromLocalStorage.bind(this);
        this.saveOptionsToLocalStorage = this.saveOptionsToLocalStorage.bind(this);
        this.onToggleClick = this.onToggleClick.bind(this);
        this.tilesTurnOn = this.tilesTurnOn.bind(this);
        this.tilesTurnOff = this.tilesTurnOff.bind(this);
    }

    setTemplate () {
        this.treeTemplate = {
            container : document.createElement('div'),
            list : document.createElement('ul'),
            item : document.createElement('li'),
            name : document.createElement('div'),
            plus : document.createElement('button')
        };

        this.previewTemplate = {
            container : document.createElement('div'),
            list : document.createElement('ul'),
            item : document.createElement('li'),
            title : document.createElement('span'),
            image : document.createElement('div'),
        };

        this.treeTemplate.container.classList.add('media-picker-tree');
        this.treeTemplate.list.classList.add('media-picker-tree__list');
        this.treeTemplate.item.classList.add('media-picker-tree__item');
        this.treeTemplate.name.classList.add('media-picker-tree__item-name');
        this.treeTemplate.plus.classList.add('media-picker-tree__plus');

        this.previewTemplate.container.classList.add('media-picker-preview');
        this.previewTemplate.list.classList.add('media-picker-preview__list');
        this.previewTemplate.item.classList.add('media-picker-preview__item');
        this.previewTemplate.item.setAttribute('tabindex', '0');
        this.previewTemplate.title.classList.add('media-picker-preview__title');
        this.previewTemplate.image.classList.add('media-picker-preview__image');
    };

    treeItem (root, itemObj) {

        const itemNode = root.treeTemplate.item.cloneNode(true);
        const nameNode = root.treeTemplate.name.cloneNode(true);

        nameNode.innerHTML = itemObj.name;
        if (itemObj.mediaArray) {

        }
        itemNode.appendChild(nameNode);
        if (!itemObj.hash) itemObj.hash = '';
        itemObj.isSelected = false;
        itemNode.mediaList = null;

        nameNode.addEventListener('click', () => root.changeLastSelected(itemNode, itemObj));
        if (this.lastContextImagePath !== null) {
            if (itemObj.path === this.lastContextImagePath) {
                root.changeLastSelected(itemNode, itemObj);
                itemObj.isSelected = true;
                itemNode.classList.add('isSelected');
            } else {
                let found = this.lastContextImagePath.indexOf(itemObj.path);
                if (found >= 0) {
                    itemNode.classList.add('isOpen');
                    itemNode.isOpen = true;
                }
            }
        }

        if (itemObj.items !== null) {
            const listNode = root.treeList(root, itemObj);
            if (itemNode.isOpen) {
                listNode.isDisabled = false;
            } else {
                listNode.classList.add('isDisabled');
                listNode.isDisabled = true;
            }
            itemNode.appendChild(listNode);

            const plusBtnNode = root.treeTemplate.plus.cloneNode(true);
            itemNode.appendChild(plusBtnNode);

            plusBtnNode.addEventListener('click', () => {
                if (listNode.isDisabled) {
                    listNode.isDisabled = false;

                    itemNode.classList.add('isOpen');
                    listNode.classList.add('go-open');
                    listNode.classList.remove('isDisabled');

                    setTimeout(() => {
                        listNode.classList.remove('go-open');
                    }, 400)
                } else {
                    listNode.isDisabled = true;

                    itemNode.classList.remove('isOpen');
                    listNode.classList.add('go-close');

                    setTimeout(() => {
                        listNode.classList.add('isDisabled');
                        listNode.classList.remove('go-close');
                    }, 400)
                }
            })
        }

        return itemNode;
    }

    treeList (root, itemObj) {
        const list = root.treeTemplate.list.cloneNode(true);

        for (let i = 0; i < itemObj.items.length; i++) {
            const item = root.treeItem(root, itemObj.items[i]);
            list.appendChild(item);
        }
        return list;
    }

    previewItem (root, itemObj) {
        const item = root.previewTemplate.item.cloneNode(true);
        item.setAttribute('data-id', itemObj.id);
        const title = root.previewTemplate.title.cloneNode(true);
        const image = root.previewTemplate.image.cloneNode(true);

        image.style.backgroundImage = ('url(' + itemObj.link + '?width=300)');
        title.innerHTML = itemObj.name;
        item.appendChild(title);
        item.appendChild(image);

        if (itemObj.id === root.lastContextImageId) {
            root.setFocusItem(item);
        }

        return item;
    }

    previewList (root, itemObj) {
        const list = root.previewTemplate.list.cloneNode(true);
        list.classList.add('isDisabled');

        for (let i = 0; i < itemObj.mediaItems.length; i++) {
            const item = root.previewItem(root, itemObj.mediaItems[i]);
            list.appendChild(item);
        }

        return list;
    }

    changeLastSelected (itemNode, itemObj) {
        if (!itemNode.isSelected) {
            itemNode.isSelected = true;
            itemNode.classList.add('isSelected');

            if (this.lastSelectedTreeItem) {
                this.lastSelectedTreeItem.isSelected = false;
                this.lastSelectedTreeItem.classList.remove('isSelected');
            }
            this.lastSelectedTreeItem = itemNode;
            this.changePreviewList(itemNode, itemObj);
        }
    };

    changePreviewList (itemNode, itemObj) {
        $.ajax({
            url: this.filesURL,
            type: 'GET',
            dataType: 'JSON',
            data: {hash: itemObj.hash, path: itemObj.path},
            async: true
        }).done((results) => {
            if (results !== null) {
                const result = results;
                itemObj.hash = result.hash;
                itemObj.mediaItems = result.items;
                if (itemObj.mediaItems !== null) {
                    if (itemNode.mediaList !== null) itemNode.mediaList.remove();
                    itemNode.mediaList = this.previewList(this, itemObj);
                    this.closeLastPreview();
                    this.showNextPreview(itemNode.mediaList);
                } else {
                    this.closeLastPreview();
                    if (itemNode.mediaList !== null) itemNode.mediaList.remove();
                    itemNode.mediaList = null;
                }
            } else {
                if (itemObj.mediaItems !== null) {
                    if (itemNode.mediaList !== null) itemNode.mediaList.remove();
                    itemNode.mediaList = this.previewList(this, itemObj);
                    this.closeLastPreview();
                    this.showNextPreview(itemNode.mediaList);
                } else {
                    this.closeLastPreview();
                }
            }
        })
    };

    closeLastPreview () {
        if (this.lastPreview) {
            let last = this.lastPreview;
            last.classList.add('go-close');

            setTimeout(() => {
                last.classList.add('isDisabled');
                last.classList.remove('go-close');
            }, 400)
        }
    };

    showNextPreview  (list)  {
        if (!list.isAppended) {
            list.isAppended = true;
            this.preview.appendChild(list);
        }

        this.lastPreview = list;
        list.classList.add('go-open');
        list.classList.remove('isDisabled');

        setTimeout(() => {
            list.classList.remove('go-open');
        }, 400);
    };

    checkStorage () {
        $.ajax({
            url: this.foldersURL,
            type: 'GET',
            dataType: 'JSON',
            data: {'hash': this.storage.hash},
            async: true
        }).done((results) => {
            if (results !== null) {
                this.storage = results;
                this.updateLocalStorage(results, true);
                if (this.isCreated) {
                    this.cleanPicker()
                }
                this.startDraw();
            } else {
                if (!this.isCreated) {
                    this.storage = this.getObjectFromLocalStorage();
                    this.startDraw();
                } else if (!this.isReopenedLastContext) {
                    this.storage = this.getObjectFromLocalStorage();
                    this.cleanPicker();
                    this.startDraw();
                }
            }
        })
    };

    checkOptionsFromLocalStorage () {
        let toggleState = localStorage.getItem('mediaLibraryOptions');
        if (toggleState !== null) {
            toggleState = JSON.parse(toggleState);
        } else {
            toggleState = {
                tails: false
            }
        }

        this.pickerOptions = toggleState;
    }

    saveOptionsToLocalStorage () {
        const opts = JSON.stringify(this.pickerOptions);
        localStorage.setItem('mediaLibraryOptions', opts);
    }

    cleanPicker() {
        if (this.tree.childNodes.length > 0) {
            for (let i = 0; i < this.tree.childNodes.length; i++) {
                this.tree.childNodes[i].remove();
            }
        }

        if (this.preview.childNodes.length > 0) {
            for (let i = 0; i < this.preview.length; i++) {
                this.preview.childNodes[i].remove();
            }
        }
    }

    updateLocalStorage (data, isObject) {
        let string = '';
        if (isObject) {
            string = JSON.stringify(data)
        } else {
            string = data;
        }
        localStorage.setItem('mediaLibrary', string)
    };

    getObjectFromLocalStorage () {
        let storage = localStorage.getItem('mediaLibrary');
        if (storage !== null) storage = JSON.parse(storage);
        return storage;
    };

    startDraw () {
        this.isCreated = true;
        this.tree.element = this.treeTemplate.container.cloneNode(true);
        this.tree.appendChild(this.tree.element);

        let list = this.treeList(this, this.storage);
        this.tree.element.appendChild(list);
    };

    createDom () {
        this.element = document.createElement('div');
        this.element.classList.add('media-picker');
        document.body.appendChild(this.element);

        this.styles = document.createElement('style');
        this.element.append(this.styles);

        this.container = document.createElement('div');
        this.container.classList.add('media-picker__inner');
        this.element.appendChild(this.container);

        this.closeBtn = document.createElement('button');
        this.closeBtn.classList.add('media-picker__close-btn');
        this.closeBtn.setAttribute('type', 'button');
        this.container.appendChild(this.closeBtn);

        this.row = document.createElement('div');
        this.row.classList.add('media-picker__row');
        this.container.appendChild(this.row);

        this.secondRow = document.createElement('div');
        this.secondRow.classList.add('media-picker__row');
        this.container.appendChild(this.secondRow);

        this.tree = document.createElement('div');
        this.tree.classList.add('media-picker__tree');
        this.row.appendChild(this.tree);

        this.preview = this.previewTemplate.container.cloneNode(true);
        this.preview.classList.add('media-picker__preview');
        this.row.appendChild(this.preview);

        this.okBtn = document.createElement('button');
        this.okBtn.setAttribute('type', 'button');
        this.okBtn.classList.add('media-picker__ok-btn');
        this.okBtn.innerHTML = 'OK';
        this.secondRow.appendChild(this.okBtn);

        this.clearBtn = document.createElement('button');
        this.clearBtn.setAttribute('type', 'button');
        this.clearBtn.classList.add('media-picker__ok-btn');
        this.clearBtn.innerHTML = 'CLEAR SELECTED';
        this.secondRow.appendChild(this.clearBtn);

        this.toggle = document.createElement('button');
        this.toggle.setAttribute('type', 'button');
        this.toggle.classList.add('media-picker__ok-btn');
        this.toggle.classList.add('media-picker__ok-btn--toggle');
        this.toggle.innerHTML = 'TILES / LIST';
        if (this.pickerOptions.tails) this.tilesTurnOn();
        this.secondRow.appendChild(this.toggle);
    }

    addListeners () {
        this.closeBtn.addEventListener('click', this.closeModal);
        this.preview.addEventListener('focus', ((e) => this.setFocusItem(e.target)), true);
        this.okBtn.addEventListener('click', this.onConfirm);
        this.element.addEventListener('keydown', this.onEnterPress);
        this.element.addEventListener('click', this.onModalZoneClick);
        this.clearBtn.addEventListener('click', this.onClearClick);
        document.body.addEventListener('keydown', this.onEscPress);
        this.toggle.addEventListener('click', this.onToggleClick);
    }

    tilesTurnOn () {
        this.toggle.classList.add('active');
        this.preview.classList.add('media-picker-preview--tiles');
    }

    tilesTurnOff () {
        this.preview.classList.remove('media-picker-preview--tiles');
        this.toggle.classList.remove('active');
    }

    onToggleClick () {
        if (!this.pickerOptions.tails) {
            this.pickerOptions.tails = true;

            this.tilesTurnOn();
        } else {
            this.pickerOptions.tails = false;

            this.tilesTurnOff();
        }
    }

    onModalZoneClick (e) {
        if (e.target === this.element) {
            this.closeModal();
            this.updateLocalStorage(this.storage, true);
        }
    }

    onEscPress (e) {
        if (e.keyCode === 27) {
            this.closeModal();
        }
    }

    onEnterPress (e) {
        if (e.keyCode === 13) {
            this.onConfirm();
        }
    }

    setFocusItem (element) {
        if (element.dataset.id) {
            if (this.lastSelectedMedia) this.lastSelectedMedia.classList.remove('isSelected');

            this.lastSelectedMedia = element;
            element.classList.add('isSelected');
        }
    }

    onClearClick () {
        this.closeModal();
        this.updateLocalStorage(this.storage, true);
        if (!this.lastContextInMany) {
            this.makeSingleNullEvent();
        } else {
            this.lastContextManyImages[this.lastContextNumberInMany] = undefined;
            this.makeInManyEvent();
        }
        this.dispatchEvent();
    }

    onConfirm () {
        this.closeModal();
        this.updateLocalStorage(this.storage, true);
        if (!this.lastContextInMany) {
            this.makeSingleEvent();
        } else {
            this.lastContextManyImages[this.lastContextNumberInMany] =
                (this.lastSelectedMedia !== null) ?
                    this.lastSelectedMedia.getAttribute('data-id')
                    :
                    undefined;
            this.makeInManyEvent();
        }
        this.dispatchEvent();
    }

    makeSingleNullEvent () {
        this.event = new CustomEvent('updateProperty', {
            detail : {
                name: this.lastContextName,
                value: undefined,
                refreshMarkup: true,
                component: this.lastContext.closest('.ktc-widget-body-wrapper')
            }
        });
    }

    makeSingleEvent() {
        this.event = new CustomEvent('updateProperty', {
            detail : {
                name: this.lastContextName,
                value: (this.lastSelectedMedia !== null) ? this.lastSelectedMedia.getAttribute('data-id') : undefined,
                refreshMarkup: true,
                component: this.lastContext.closest('.ktc-widget-body-wrapper')
            }
        });
    }

    makeInManyEvent () {
        this.event = new CustomEvent('updateProperty', {
            detail : {
                name: this.lastContextName,
                value: JSON.stringify(this.lastContextManyImages),
                refreshMarkup: true,
                component: this.lastContext.closest('.ktc-widget-body-wrapper')
            }
        });
    }

    dispatchEvent () {
        if (this.event !== null) {
            this.lastContext.dispatchEvent(this.event);
            window.dispatchEvent(this.event);
        } else {
            console.log('event object is null');
        }
    }

    openPicker (options) {
        this.event = null;
        this.setOptions(options);
        this.checkStorage();
        this.showModal();
    };

    setOptions (options) {
        if (this.lastContext === options.context) {
            this.isReopenedLastContext = true;
        } else {
            this.isReopenedLastContext = false;
        }

        this.lastContext = options.context;
        this.lastContextName = options.contextName;

        if ((options.contextImage) && (options.contextImage !== '')) {
            this.lastContextImage = options.contextImage;
        } else {
            this.lastContextImage = null;
        }

        if ((options.contextImagePath) && (options.contextImagePath !== '')) {
            this.lastContextImagePath =  options.contextImagePath;
        } else {
            this.lastContextImagePath = null;
        }

        if ((options.contextImageId) && (options.contextImageId !== '')) {
            this.lastContextImageId = options.contextImageId;
        } else {
            this.lastContextImageId = null;
        }

        if (options.inMany) {
            this.lastContextInMany = true;
            this.lastContextManyImages = options.images;
            this.lastContextNumberInMany = options.imageNum;
        } else {
            this.lastContextInMany = false;
            this.lastContextManyImages = null;
            this.lastContextNumberInMany = null;
        }
    }

    setDynamicStyles () {
        const height = this.element.clientHeight;
        const modalPaddings = 100;
        const innerPaddings = 40;
        this.styles.innerHTML =
            '.media-picker__inner { height: ' + (height - modalPaddings) + 'px; max-height: ' + (height - modalPaddings) +'px} \n' +
            '.media-picker-preview { height : ' + (height - modalPaddings - innerPaddings)+ 'px; } \n';
    }

    showModal () {
        if (!this.isOpen) {
            document.body.style.overflow = 'hidden';
            this.element.classList.add('go-show');
            this.element.classList.add('isOpen');
            this.setDynamicStyles();

            setTimeout(() => {
                this.isOpen = true;
                this.element.classList.remove('go-show');

            }, 500)
        }
    };

    closeModal () {
        if (this.isOpen) {
            document.body.style.overflow = '';
            this.element.classList.add('go-hide');
            this.styles.innerHTML = '';
            this.saveOptionsToLocalStorage();

            setTimeout(() => {
                this.isOpen = false;
                this.element.classList.remove('isOpen');
                this.element.classList.remove('go-hide');
            }, 500)
        }
    };
}


(function () {
    window.kentico.pageBuilder.registerInlineEditor("media-editor", {
        init: function (options) {
            if (!window.kentico.mediaPicker) {
                window.kentico.mediaPicker = new mediaPicker();
                let editor = options.editor;
                let foldersURL = editor.getAttribute('data-foldersurl');
                let filesURL = editor.getAttribute('data-filesurl');
                window.kentico.mediaPicker.init(foldersURL, filesURL);
            }

            let button = options.editor.querySelector('.media-picker-button');

            const inMany = button.getAttribute('data-images') ? true : false;

            let contextOptions = {
                context : options.editor,
                contextName : options.propertyName,
                contextImage : button.getAttribute('data-image-src'),
                contextImagePath : button.getAttribute('data-image-path'),
                contextImageId : button.getAttribute('data-image-id'),
                inMany : inMany,
                images : inMany ? JSON.parse(button.getAttribute('data-images')) : null,
                imageNum : inMany ? button.getAttribute('data-imagenum') : null
            };

            button.addEventListener('click', () => {
                window.kentico.mediaPicker.openPicker(contextOptions)
            })
        },

        destroy: function (options) {
        },

        dragStart: function (options) {
        }
    });
})();
