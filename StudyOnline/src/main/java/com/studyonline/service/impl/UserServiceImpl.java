package com.studyonline.service.impl;

import com.studyonline.entity.User;
import com.studyonline.model.UsersListModel;
import com.studyonline.repository.UserRepository;
import com.studyonline.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserServiceImpl implements UserService {

    @Autowired
    private UserRepository userRepository;

    @Override
    public List<User> getAllUser() {
        return userRepository.findAll();
    }
}
