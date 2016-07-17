#!/usr/bin/env python3
# -*- coding: utf-8 -*-

'''
Configuration

Usage:
from config import configs
'''

__author__ = 'Anthony Fu'

import sys
sys.path.append("configs")

class IndexedDict(dict):
    '''
    Simple dict but support access as x.y style.
    '''
    def __init__(self,d=None, **kw):
        super(IndexedDict, self).__init__(**kw)
        if d:
            for k, v in d.items():
                self[k] = IndexedDict(v) if isinstance(v, dict) else v

    def __getattr__(self, key):
        try:
            return self[key]
        except KeyError:
            raise AttributeError(r"'IndexedDict' object has no attribute '%s'" % key)

    def __setattr__(self, key, value):
        self[key] = value

def dictMerge(defaults, override):
    '''
    Merge two dicts into one.
    '''
    r = {}
    for k, v in defaults.items():
        if k in override:
            if isinstance(v, dict):
                r[k] = dictMerge(v, override[k])
            else:
                r[k] = override[k]
        else:
            r[k] = v
    return r

import config_default
try:
    import config_override
    configs = dictMerge(config_default.configs, config_override.configs)
except ImportError:
    configs = config_default.configs

configs = IndexedDict(configs)